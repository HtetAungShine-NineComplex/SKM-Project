using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using THZ;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManagerShan : MonoBehaviour
{
    private List<RoomUserItem> _userItems;

    [SerializeField] private GameObject _turnTxt;
    [SerializeField] private RoomUserItem _roomUserItemPrefab;
    [SerializeField] private Transform _roomUserRoot;
    [SerializeField] private TMP_Text _roomNameTxt;
    [SerializeField] private Button _hitBtn;
    [SerializeField] private Button _standBtn;
    [SerializeField] private Button _startBtn;
    [SerializeField] private TMP_Text _gameCDTxt;

    [SerializeField] private Transform _mainUserPos;

    [SerializeField] private Transform[] userPositions;

    private Room _currentRoom;

    private void Awake()
    {
        _userItems = new List<RoomUserItem>();
    }

    private void Start()
    {
        
    }

    public void Initialize()
    {
        _currentRoom = Managers.NetworkManager.SmartFox.LastJoinedRoom;
        _roomNameTxt.text = _currentRoom.Name;

        ListenServerEvents();
        PopulateUserList();

        ToggleGameplayBtns(false);
    }

    private void ListenServerEvents()
    {
        Managers.NetworkManager.UserEnterRoom += OnUserEnterRoom;
        Managers.NetworkManager.StartCurrentTurn += OnStartCurrentTurn;
        Managers.NetworkManager.Owner += OnOwner;
        Managers.NetworkManager.GameStarted += OnGameStarted;
        Managers.NetworkManager.Countdown += OnCountdown;
        Managers.NetworkManager.PlayerDrawCard += OnDrawCard;
        Managers.NetworkManager.PlayerWin += OnWinEvent;
        Managers.NetworkManager.PlayerLose += OnLoseEvent;
        Managers.NetworkManager.PlayerTotalValue += OnPlayerTotalValue;
        Managers.NetworkManager.PlayerDo += OnPlayerDo;
        Managers.NetworkManager.PlayerHandCards += OnPlayerHandCard;
    }

    public void RemovenServerEvents()
    {
        Managers.NetworkManager.UserEnterRoom -= OnUserEnterRoom;
        Managers.NetworkManager.StartCurrentTurn -= OnStartCurrentTurn;
        Managers.NetworkManager.Owner -= OnOwner;
        Managers.NetworkManager.GameStarted -= OnGameStarted;
        Managers.NetworkManager.Countdown -= OnCountdown;
        Managers.NetworkManager.PlayerDrawCard -= OnDrawCard;
        Managers.NetworkManager.PlayerWin -= OnWinEvent;
        Managers.NetworkManager.PlayerLose -= OnLoseEvent;
        Managers.NetworkManager.PlayerTotalValue -= OnPlayerTotalValue;
        Managers.NetworkManager.PlayerDo -= OnPlayerDo;
        Managers.NetworkManager.PlayerHandCards -= OnPlayerHandCard;
    }

    private void AddTwoCardToAllPlayers()
    {
        foreach (RoomUserItem item in _userItems)
        {
            item.AddBlankCards();
            item.AddBlankCards();
        }
    }

    private void OnUserEnterRoom(User user)
    {
        AddUserItem(user, _userItems.Count);
    }

    private void PopulateUserList()
    {
        List<User> users = new List<User>(_currentRoom.UserList);
        users.Remove(Managers.NetworkManager.SmartFox.MySelf);

        AddUserItem(Managers.NetworkManager.SmartFox.MySelf, 0);

        for (int i = 0; i < users.Count; i++)
        {
            AddUserItem(users[i], i);
        }
    }

    private void AddUserItem(User user,int index)
    {
        RoomUserItem roomUserItem = Instantiate(_roomUserItemPrefab);
        roomUserItem.SetName(user.Name);
        roomUserItem.SetId(user.Id);

        roomUserItem.transform.SetParent(_roomUserRoot, false);

        if(roomUserItem.ID == Managers.NetworkManager.SmartFox.MySelf.Id)
        {
            roomUserItem.transform.localPosition = _mainUserPos.localPosition;
            _userItems.Add(roomUserItem);
            return;
        }

        if (index < userPositions.Length)
        {
            roomUserItem.transform.localPosition = userPositions[index].localPosition;
        }
        else
        {
            //something for spectator           
        }

        _userItems.Add(roomUserItem);
    }


    public void RemoveUserItem(RoomUserItem userItem)
    {
        _userItems.Remove(userItem);
    }

    public RoomUserItem GetUserItemByID(int id)
    {
        foreach (RoomUserItem userItem in _userItems)
        {
            if (userItem.ID == id)
            {
                return userItem;
            }
        }

        return null;
    }

    public void OnStartCurrentTurn(ISFSObject sfsObj) 
    {
        //on player turn change, check if it is my turn or not by id
        int id = sfsObj.GetInt(GameConstants.USER_ID);

        if(Managers.NetworkManager.SmartFox.MySelf.Id == id)
        {
            ShowObjectForSecs(_turnTxt, 2);
            ToggleGameplayBtns(true);
        }
        else
        {
            ToggleGameplayBtns(false);
        }
        
    }

    public void ToggleGameplayBtns(bool show)
    {
        if(show)
        {
            _hitBtn.onClick.AddListener(() => DrawCard());
            _standBtn.onClick.AddListener(() => Stand());
        }
        else
        {
            _hitBtn.onClick.RemoveAllListeners();
            _standBtn.onClick.RemoveAllListeners();
        }

        _hitBtn.gameObject.SetActive(show);
        _standBtn.gameObject.SetActive(show);
    }

    private void StartGame() //send to server when the room owner press start
    {
        if(_userItems.Count < 2)
        {
            Debug.Log("Need at least 2 players to start the game");
            return;
        }
        _startBtn.gameObject.SetActive(false);
        ISFSObject data = new SFSObject();
        ExtensionRequest request = new ExtensionRequest(GameConstants.START_GAME, data, _currentRoom);
        Managers.NetworkManager.SendRequest(request);
    }

    public void DrawCard() //send to server when this client draw a card
    {
        ISFSObject data = new SFSObject();
        data.PutInt(GameConstants.USER_ID, Managers.NetworkManager.SmartFox.MySelf.Id);
        ExtensionRequest request = new ExtensionRequest(GameConstants.DRAW_CARD, data, _currentRoom);
        Managers.NetworkManager.SendRequest(request);

        ToggleGameplayBtns(false);
    }

    public void Stand() //send to server when this client stand
    {
        ISFSObject data = new SFSObject();
        ExtensionRequest request = new ExtensionRequest(GameConstants.STAND, data, _currentRoom);
        Managers.NetworkManager.SendRequest(request);

        ToggleGameplayBtns(false);
    }

    public void OnOwner(ISFSObject sfsObj) //sent from the server when the room owner is set
    {
        int id = sfsObj.GetInt(GameConstants.USER_ID);
        GetUserItemByID(id).IsBanker();
        if(id == Managers.NetworkManager.SmartFox.MySelf.Id)
        {
            _startBtn.onClick.AddListener(() => StartGame());
            _startBtn.gameObject.SetActive(true);
        }
    }

    private void OnCountdown(ISFSObject sfsObj) //countdown event before game start
    {
        int countdown = sfsObj.GetInt(GameConstants.COUNTDOWN);
        _gameCDTxt.gameObject.SetActive(true);
        _gameCDTxt.text = "Game will start in: " + countdown;
        ResetGame();
    }

    private void OnGameStarted(ISFSObject sfsObj) //this will receive when the game started
    {
        ResetGame();
        AddTwoCardToAllPlayers();
        _gameCDTxt.gameObject.SetActive(false);
    }

    private void OnPlayerDo(ISFSObject sfsObj) //this will receive when a player do
    {
        string[] playerCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        int playerID = sfsObj.GetInt(GameConstants.USER_ID);

        Debug.Log($"{GetUserItemByID(playerID).Name} is DO with Cards [{playerCards[0]}, {playerCards[1]}] and total value of {totalValue}");

        GetUserItemByID(playerID).SetTotalValue(totalValue);
        GetUserItemByID(playerID).PlayerDo(totalValue);
        GetUserItemByID(playerID).UpdateAllCards(playerCards);
    }

    private void OnDrawCard(ISFSObject sfsObj) //this will receive when a player hit
    {
        int drawerID = sfsObj.GetInt(GameConstants.USER_ID);

        if(drawerID == Managers.NetworkManager.SmartFox.MySelf.Id )
        {
            string drawnCardName = sfsObj.GetUtfString(GameConstants.CARD_NAME);
            int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
            Debug.Log($"{GetUserItemByID(drawerID).Name} draw Card [{drawnCardName}] and total value is {totalValue}");
            GetUserItemByID(drawerID).SetTotalValue(totalValue);
            GetUserItemByID(drawerID).AddCard(drawnCardName);
        }
        else
        {
            Debug.Log($"{GetUserItemByID(drawerID).Name} draw a card ..");
            GetUserItemByID(drawerID).AddBlankCards();
        }
    }

    private void OnPlayerHandCard(ISFSObject sfsObj) //this will receive when server send u ur own hand cards
    {
        int id = sfsObj.GetInt(GameConstants.USER_ID);

        string[] handCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        GetUserItemByID(id).SetTotalValue(totalValue);

        GetUserItemByID(id).UpdateAllCards(handCards);
    }

    private void OnWinEvent(ISFSObject sfsObj) //this will receive when a player win
    {
        int id = sfsObj.GetInt(GameConstants.USER_ID);
        string[] handCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);


        GetUserItemByID(id).UpdateAllCards(handCards);

        GetUserItemByID(id).SetTotalValue(totalValue);
        GetUserItemByID(id).WinLose(true);
        ToggleGameplayBtns(false);
    }

    private void OnLoseEvent(ISFSObject sfsObj) //this will receive when a player lose
    {
        int id = sfsObj.GetInt(GameConstants.USER_ID);
        string[] handCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);


        GetUserItemByID(id).UpdateAllCards(handCards);

        GetUserItemByID(id).SetTotalValue(totalValue);
        GetUserItemByID(id).WinLose(false);
        ToggleGameplayBtns(false);
    }

    private void OnPlayerTotalValue(ISFSObject sfsObj)
    {
        int id = sfsObj.GetInt(GameConstants.USER_ID);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        Debug.Log("Player Total Value : " + totalValue);
        GetUserItemByID(id).SetTotalValue(totalValue);
    }

    private void ShowObjectForSecs(GameObject obj, int duration)
    {
        StartCoroutine(ShowObject(obj, duration));
    }

    IEnumerator ShowObject(GameObject obj, int duration)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(duration);

        obj.SetActive(false);
    }

    void ResetGame()
    {
        ToggleGameplayBtns(false);

        foreach (RoomUserItem item in _userItems)
        {
            item.Reset();
        }
    }
}
