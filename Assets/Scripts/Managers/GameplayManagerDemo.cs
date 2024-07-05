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

public class GameplayManagerDemo : MonoBehaviour
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

    private Room _currentRoom;

    private void Awake()
    {
        _userItems = new List<RoomUserItem>();
    }

    private void Start()
    {
        ToggleGameplayBtns(false);
    }

    public void Initialize()
    {
        _currentRoom = Managers.NetworkManager.SmartFox.LastJoinedRoom;
        _roomNameTxt.text = _currentRoom.Name;

        ListenServerEvents();
        PopulateUserList();
    }

    private void ListenServerEvents()
    {
        Managers.NetworkManager.UserEnterRoom += OnUserEnterRoom;
        Managers.NetworkManager.StartCurrentTurn += OnStartCurrentTurn;
        Managers.NetworkManager.Owner += OnOwner;
        Managers.NetworkManager.GameStarted += OnGameStarted;
        Managers.NetworkManager.Countdown += OnCountdown;
        Managers.NetworkManager.PlayerHit += OnHit;
        Managers.NetworkManager.PlayerWin += OnWinEvent;
        Managers.NetworkManager.PlayerLose += OnLoseEvent;
        Managers.NetworkManager.PlayerTotalValue += OnPlayerTotalValue;
    }

    private void OnUserEnterRoom(User user)
    {
        AddUserItem(user, _userItems.Count);
    }

    private void PopulateUserList()
    {
        List<User> users = _currentRoom.UserList;

        for (int i = 0; i < users.Count; i++)
        {
            AddUserItem(users[i], i);
        }
    }

    private Vector2[] userPositions = new Vector2[]
{
    new Vector2(0, -400),   // Main user
    new Vector2(-550, 315), // Second user
    new Vector2(-700, 160), // Third user
    new Vector2(-700, -200),// Fourth user
    new Vector2(-450, -360),// Fifth user
    new Vector2(550, 315),  // Sixth user
    new Vector2(700, 160),  // Seventh user
    new Vector2(700, -200), // Eighth user
    new Vector2(450, -360)  // Ninth user
};

    private void AddUserItem(User user,int index)
    {
        RoomUserItem roomUserItem = Instantiate(_roomUserItemPrefab);
        roomUserItem.SetName(user.Name);
        roomUserItem.SetId(user.Id);

        roomUserItem.transform.SetParent(_roomUserRoot, false);

        if (index < userPositions.Length)
        {
            roomUserItem.transform.localPosition = userPositions[index];
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
            _hitBtn.onClick.AddListener(() => Hit());
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

    public void Hit() //send to server when this client hit
    {
        ISFSObject data = new SFSObject();
        data.PutInt(GameConstants.USER_ID, Managers.NetworkManager.SmartFox.MySelf.Id);
        ExtensionRequest request = new ExtensionRequest(GameConstants.HIT, data, _currentRoom);
        Managers.NetworkManager.SendRequest(request);
    }

    public void Stand() //send to server when this client stand
    {
        ISFSObject data = new SFSObject();
        ExtensionRequest request = new ExtensionRequest(GameConstants.STAND, data, _currentRoom);
        Managers.NetworkManager.SendRequest(request);
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
        
        _gameCDTxt.gameObject.SetActive(false);
    }

    private void OnHit(ISFSObject sfsObj) //this will receive when a player hit
    {
        string drawnCardName = sfsObj.GetUtfString(GameConstants.CARD_NAME);
        int drawnCardValue = sfsObj.GetInt(GameConstants.CARD_VALUE);
        int suit = sfsObj.GetInt(GameConstants.SUIT);
        bool isAce = sfsObj.GetBool(GameConstants.IS_ACE);

        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        int drawerID = sfsObj.GetInt(GameConstants.USER_ID);

        //Debug.Log("Drawn Card Name: " + drawnCardName + " Drawn Card Value: " + drawnCardValue + " Suit: " + (Suit)suit + " Is Ace: " + isAce + " Total Value: " + totalValue);

        GetUserItemByID(drawerID).SetTotalValue(totalValue);
    }

    private void OnWinEvent(ISFSObject sfsObj) //this will receive when a player win
    {
        int id = sfsObj.GetInt(GameConstants.USER_ID);
        GetUserItemByID(id).WinLose(true);
        ToggleGameplayBtns(false);
    }

    private void OnLoseEvent(ISFSObject sfsObj) //this will receive when a player lose
    {
        int id = sfsObj.GetInt(GameConstants.USER_ID);
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
