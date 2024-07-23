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
        Managers.NetworkManager.BotJoined += OnBotJoined;
        Managers.NetworkManager.StartCurrentTurn += OnStartCurrentTurn;
        Managers.NetworkManager.Owner += OnOwner;
        Managers.NetworkManager.Banker += OnBanker;
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
        Managers.NetworkManager.Banker -= OnBanker;
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

    private void OnBotJoined(ISFSObject sfsObj)
    {
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);

        AddUserItem(playerName, _userItems.Count);
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

    private void AddUserItem(string name, int index) //bots
    {
        RoomUserItem roomUserItem = Instantiate(_roomUserItemPrefab);
        roomUserItem.SetName(name);

        roomUserItem.transform.SetParent(_roomUserRoot, false);

        if (roomUserItem.ID == Managers.NetworkManager.SmartFox.MySelf.Id)
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

    public RoomUserItem GetUserItemByName(string name)
    {
        foreach (RoomUserItem userItem in _userItems)
        {
            if (userItem.Name == name)
            {
                return userItem;
            }
        }

        return null;
    }

    public void OnStartCurrentTurn(ISFSObject sfsObj) 
    {
        //on player turn change, check if it is my turn or not by id
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);

        if (Managers.NetworkManager.SmartFox.MySelf.Name == playerName)
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
        data.PutUtfString(GameConstants.USER_NAME, Managers.NetworkManager.SmartFox.MySelf.Name);
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
        Debug.Log("Owner received");
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);
        //GetUserItemByID(id).IsBanker();
        if (playerName == Managers.NetworkManager.SmartFox.MySelf.Name)
        {
            _startBtn.onClick.AddListener(() => StartGame());
            _startBtn.gameObject.SetActive(true);
        }
    }

    public void OnBanker(ISFSObject sfsObj)
    {
        Debug.Log("On Banker");

        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);

        foreach (RoomUserItem item in _userItems)
        {
            if (item.Name == playerName)
            {
                item.IsBanker();
            }
            else
            {
                item.IsPlayer();
            }
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
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);

        Debug.Log($"{playerName} is DO with Cards [{playerCards[0]}, {playerCards[1]}] and total value of {totalValue}");

        GetUserItemByName(playerName).SetTotalValue(totalValue);
        GetUserItemByName(playerName).PlayerDo(totalValue);
        GetUserItemByName(playerName).UpdateAllCards(playerCards);
    }

    private void OnDrawCard(ISFSObject sfsObj) //this will receive when a player hit
    {
        string drawerName = sfsObj.GetUtfString(GameConstants.USER_NAME);

        if (drawerName == Managers.NetworkManager.SmartFox.MySelf.Name )
        {
            string drawnCardName = sfsObj.GetUtfString(GameConstants.CARD_NAME);
            int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
            Debug.Log($"{GetUserItemByName(drawerName).Name} draw Card [{drawnCardName}] and total value is {totalValue}");
            GetUserItemByName(drawerName).SetTotalValue(totalValue);
            GetUserItemByName(drawerName).AddCard(drawnCardName);
        }
        else
        {
            Debug.Log($"{drawerName} draw a card ..");
            GetUserItemByName(drawerName).AddBlankCards();
        }
    }

    private void OnPlayerHandCard(ISFSObject sfsObj) //this will receive when server send u ur own hand cards
    {
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);

        string[] handCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);

        
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        GetUserItemByName(playerName).SetTotalValue(totalValue);

        GetUserItemByName(playerName).UpdateAllCards(handCards);
    }

    private void OnWinEvent(ISFSObject sfsObj) //this will receive when a player win
    {
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);
        string[] handCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);

        foreach (string handCard in handCards)
        {
            Debug.Log(playerName + " " + handCard);
        }

        GetUserItemByName(playerName).UpdateAllCards(handCards);

        GetUserItemByName(playerName).SetTotalValue(totalValue);
        GetUserItemByName(playerName).WinLose(true);
        ToggleGameplayBtns(false);
    }

    private void OnLoseEvent(ISFSObject sfsObj) //this will receive when a player lose
    {
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);
        string[] handCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);

        foreach (string handCard in handCards)
        {
            Debug.Log(playerName + " " + handCard);
        }

        GetUserItemByName(playerName).UpdateAllCards(handCards);

        GetUserItemByName(playerName).SetTotalValue(totalValue);
        GetUserItemByName(playerName).WinLose(false);
        ToggleGameplayBtns(false);
    }

    private void OnPlayerTotalValue(ISFSObject sfsObj)
    {
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        Debug.Log("Player Total Value : " + totalValue);
        GetUserItemByName(playerName).SetTotalValue(totalValue);
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
