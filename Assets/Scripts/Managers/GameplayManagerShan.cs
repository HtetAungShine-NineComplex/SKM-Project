using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using THZ;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManagerShan : MonoBehaviour
{
    private List<RoomUserItem> _userItems;

    [SerializeField] private GameObject _turnTxt;
    [SerializeField] private GameObject _pleaseWaitTxt;
    [SerializeField] private RoomUserItem _roomUserItemPrefab;
    [SerializeField] private Transform _roomUserRoot;
    [SerializeField] private TMP_Text _roomNameTxt;
    [SerializeField] private Button _hitBtn;
    [SerializeField] private Button _standBtn;
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _betBtn;
    [SerializeField] private TMP_Text _gameCDTxt;
    [SerializeField] private TMP_Text _currentPlayerTurnTxt;
    [SerializeField] private TMP_Text _bankAmountTxt;

    [SerializeField] private BankDisplay _bankDisplay;

    [SerializeField] private BetPanelAmountController _betAmountCtrlr;

    //[SerializeField] private Transform _mainUserPos;

    //[SerializeField] private Transform[] userPositions;
    [SerializeField] private PlayerPos[] playerPositions;

    private Room _currentRoom;

    int bet1;
    int bet2;
    int bet3;
    int maxBet;

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
        //PopulateUserList();

        ToggleGameplayBtns(false);
    }

    
    private void ListenServerEvents()
    {
        Managers.NetworkManager.UserEnterRoom += OnUserEnterRoom;
        Managers.NetworkManager.UserLeaveRoom += OnUserLeaveRoom;
        Managers.NetworkManager.BotJoined += OnBotJoined;
        Managers.NetworkManager.StartCurrentTurn += OnStartCurrentTurn;
        Managers.NetworkManager.Owner += OnOwner;
        Managers.NetworkManager.Banker += OnBanker;
        Managers.NetworkManager.GameStarted += OnGameStarted;
        Managers.NetworkManager.BetStarted += OnBetStarted;
        Managers.NetworkManager.Countdown += OnCountdown;
        Managers.NetworkManager.PlayerDrawCard += OnDrawCard;
        Managers.NetworkManager.PlayerWin += OnWinEvent;
        Managers.NetworkManager.PlayerLose += OnLoseEvent;
        Managers.NetworkManager.PlayerTotalValue += OnPlayerTotalValue;
        Managers.NetworkManager.PlayerDo += OnPlayerDo;
        Managers.NetworkManager.PlayerHandCards += OnPlayerHandCard;
        Managers.NetworkManager.RoomPlayerList += OnRoomPlayerList;
        Managers.NetworkManager.PlayerBet += OnPlayerBet;
        Managers.NetworkManager.PleaseWait += OnPleaseWait;
    }

    public void RemovenServerEvents()
    {
        Managers.NetworkManager.UserEnterRoom -= OnUserEnterRoom;
        Managers.NetworkManager.UserLeaveRoom -= OnUserLeaveRoom;
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
        Managers.NetworkManager.RoomPlayerList -= OnRoomPlayerList;
        Managers.NetworkManager.PlayerBet -= OnPlayerBet;
        Managers.NetworkManager.PleaseWait -= OnPleaseWait;
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
        //AddUserItem(user, _userItems.Count);
    }

    private void OnUserLeaveRoom(User user)
    {

    }

    private void OnBotJoined(ISFSObject sfsObj)
    {
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);

        // AddUserItem(playerName, _userItems.Count);
    }

    /*private void PopulateUserList()
    {
        *//*List<User> users = new List<User>(_currentRoom.UserList);
        users.Remove(Managers.NetworkManager.SmartFox.MySelf);

        AddUserItem(Managers.NetworkManager.SmartFox.MySelf, 0);

        for (int i = 0; i < users.Count; i++)
        {
            AddUserItem(users[i], i);
        }*//*
    }*/

    private void OnRoomPlayerList(ISFSObject sfsObj)
    {
        ISFSArray sfsArr = sfsObj.GetSFSArray(GameConstants.USER_ARRAY);

        int myIndex = 0;

        for (int i = 0; i < sfsArr.Size(); i++)
        {
            ISFSObject obj = sfsArr.GetSFSObject(i);

            if (obj.GetUtfString(GameConstants.USER_NAME) == Managers.NetworkManager.SmartFox.MySelf.Name)
            {
                myIndex = i;
                break;
            }
        }

        if (_userItems.Count > 0)
        {
            foreach (RoomUserItem item in _userItems)
            {
                Destroy(item.gameObject);
            }
        }

        _userItems.Clear();

        AddUserItem(myIndex, sfsArr);
    }

    private void AddUserItem(int myIndex, ISFSArray sfsArr)
    {
        /*for (int i = 0; i < nameArr.Length; i++)
        {
            int ind = (myIndex + i) % nameArr.Length;

            RoomUserItem roomUserItem = Instantiate(_roomUserItemPrefab);
            roomUserItem.SetName(nameArr[ind]);
            roomUserItem.transform.SetParent(_roomUserRoot, false);
            roomUserItem.transform.localPosition = userPositions[i].localPosition;
            _userItems.Add(roomUserItem);
            // roomUserItem.SetId(user.Id);
        }*/

        for (int i = 0; i < sfsArr.Size(); i++)
        {
            int ind = (myIndex + i) % sfsArr.Size();

            ISFSObject obj = sfsArr.GetSFSObject(ind);

            RoomUserItem roomUserItem = Instantiate(_roomUserItemPrefab);
            roomUserItem.SetName(obj.GetUtfString(GameConstants.USER_NAME));
            roomUserItem.SetAmount(obj.GetInt(GameConstants.TOTAL_AMOUNT));
            roomUserItem.transform.SetParent(_roomUserRoot, false);
            //roomUserItem.transform.localPosition = userPositions[i].localPosition;
            roomUserItem.transform.localPosition = playerPositions[i].rectTransform.localPosition;
            playerPositions[i].SetCurrentUser(roomUserItem);
            _userItems.Add(roomUserItem);
        }
    }

    /*private void AddUserItem(User user,int index)
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
    }*/

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

        foreach (RoomUserItem item in _userItems)
        {
            item.ToggleLoadingObject(false);

            if (item.Name == playerName)
            {
                item.StartTurn();
            }
            else
            {
                item.EndTurn();
            }
        }

        if (Managers.NetworkManager.SmartFox.MySelf.Name == playerName)
        {
            //ShowObjectForSecs(_turnTxt, 2);
            ToggleGameplayBtns(true);
        }
        else
        {

            ToggleGameplayBtns(false);
        }
        _currentPlayerTurnTxt.text = playerName + "'s Turn...";
    }

    public void ToggleGameplayBtns(bool show)
    {
        if (show)
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
        Debug.Log("user tried to start!"); //srat
        if (_userItems.Count < 2)
        {
            Debug.Log("Need at least 2 players to start the game");
            return;
        }
        _startBtn.gameObject.SetActive(false);
        ISFSObject data = new SFSObject();
        ExtensionRequest request = new ExtensionRequest(GameConstants.START_GAME, data, _currentRoom);
        Managers.NetworkManager.SendRequest(request);
    }

    public void Bet(int betBtnID) //send to server when this client bet
    {
        int betAmount = 0;
        switch (betBtnID)
        {
            case 0:
                betAmount = bet1;
                break;

            case 1:
                betAmount = bet2;
                break;

            case 2:
                betAmount = bet3;
                break;

            case 3:
                betAmount = maxBet;
                break;

            default:
                break;
        }

        ISFSObject data = new SFSObject();
        data.PutUtfString(GameConstants.USER_NAME, Managers.NetworkManager.SmartFox.MySelf.Name);
        data.PutInt(GameConstants.BET_AMOUNT, betAmount);
        //data.PutUtfString(GameConstants.BET_AMOUNT, 1000);
        ExtensionRequest request = new ExtensionRequest(GameConstants.BET, data, _currentRoom);
        Managers.NetworkManager.SendRequest(request);

        _betAmountCtrlr.gameObject.SetActive(false);
    }

    public void DrawCard() //send to server when this client draw a card
    {
        Managers.AudioManager.PlayWillDrawCardClip();
        ISFSObject data = new SFSObject();
        data.PutUtfString(GameConstants.USER_NAME, Managers.NetworkManager.SmartFox.MySelf.Name);
        ExtensionRequest request = new ExtensionRequest(GameConstants.DRAW_CARD, data, _currentRoom);
        Managers.NetworkManager.SendRequest(request);

        ToggleGameplayBtns(false);
    }

    public void Stand() //send to server when this client stand
    {
        Managers.AudioManager.PlayWontDrawCardClip();
        ISFSObject data = new SFSObject();
        ExtensionRequest request = new ExtensionRequest(GameConstants.STAND, data, _currentRoom);
        Managers.NetworkManager.SendRequest(request);

        ToggleGameplayBtns(false);
    }

    public void LeaveRoomToMainMenu() //send to server when this client stand
    {
        Room currentRoom = Managers.NetworkManager.SmartFox.LastJoinedRoom;
        if (currentRoom != null)
        {
            Managers.NetworkManager.SmartFox.Send(new LeaveRoomRequest(currentRoom));
            Debug.Log("Leaving room: " + currentRoom.Name);
            Managers.UIManager.ShowUI(UIs.UIMainMenu);
        }
        else
        {
            Debug.LogWarning("No room to leave.");
        }
    }

    public void LeaveRoomToLobby() //send to server when this client stand
    {
        Room currentRoom = Managers.NetworkManager.SmartFox.LastJoinedRoom;
        if (currentRoom != null)
        {
            Managers.NetworkManager.SmartFox.Send(new LeaveRoomRequest(currentRoom));
            Debug.Log("Leaving room: " + currentRoom.Name);
            Managers.UIManager.ShowUI(UIs.UIRoom);
        }
        else
        {
            Debug.LogWarning("No room to leave.");
        }
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
        int totalAmount = sfsObj.GetInt(GameConstants.TOTAL_AMOUNT);
        int bankAmount = sfsObj.GetInt(GameConstants.BANK_AMOUNT);

        _bankAmountTxt.text = bankAmount.ToString();
        _bankDisplay.DisplayNumber(bankAmount.ToString());

        foreach (RoomUserItem item in _userItems)
        {
            if (item.Name == playerName)
            {
                item.IsBanker();
                item.SetAmount(totalAmount);
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
        if(countdown == 8)
        {
            Managers.AudioManager.PlayStartingNewGameClip();
        }

        Managers.AudioManager.PlayTimeTickingClip();
        ResetGame();
    }

    private void OnBetStarted(ISFSObject sfsObj) //this will receive when the bet state started
    {
        Managers.AudioManager.PlayPlayersAreBettingClip();
        ResetGame();
        //AddTwoCardToAllPlayers();
        _gameCDTxt.gameObject.SetActive(false);

        bool isMeBank = false;
        foreach (RoomUserItem item in _userItems)
        {
            if (!item.IsBank)
            {
                item.StartBet();
            }
            else
            {
                if(item.Name == GlobalManager.Instance.GetSfsClient().MySelf.Name)
                {
                    isMeBank = true;
                }
            }
        }

        

        int bankAmount = sfsObj.GetInt(GameConstants.BANK_AMOUNT);
        _bankAmountTxt.text = bankAmount.ToString();

        if (isMeBank)
        {
            return;
        }

        bet1 = sfsObj.GetInt("bet1");
        bet2 = sfsObj.GetInt("bet2");
        bet3 = sfsObj.GetInt("bet3");
        maxBet = bankAmount;

        _betAmountCtrlr.SetBtnAmounts(bet1, bet2, bet3, bankAmount);
        _betAmountCtrlr.gameObject.SetActive(true);

        Debug.Log("Bank : " + bankAmount);
        _bankAmountTxt.text = bankAmount.ToString();
        _bankDisplay.DisplayNumber(bankAmount.ToString());

    }

    public void OnPleaseWait(ISFSObject sfsObj) //send from server when a client
    {
        _pleaseWaitTxt.SetActive(true);
    }

    public void OnPlayerBet(ISFSObject sfsObj) //send from server when a client bet
    {
        string name = sfsObj.GetUtfString(GameConstants.USER_NAME);
        int betAmount = sfsObj.GetInt(GameConstants.BET_AMOUNT);
        int totalAmount = sfsObj.GetInt(GameConstants.TOTAL_AMOUNT);

        Debug.Log("Player : " + name + " bet " + betAmount + " and " + totalAmount);
        GetUserItemByName(name).SetAmount(totalAmount);
        GetUserItemByName(name).SetBetAmount(betAmount);
        GetUserItemByName(name).OnBet(betAmount);
        GetUserItemByName(name).EndBet();

        if(name == GlobalManager.Instance.GetSfsClient().MySelf.Name)
        {
            _betAmountCtrlr.gameObject.SetActive(false);
        }
    }

    private void OnGameStarted(ISFSObject sfsObj) //this will receive when the game started
    {
        //ResetGame();
        //AddTwoCardToAllPlayers();
        _gameCDTxt.gameObject.SetActive(false);
        foreach (RoomUserItem item in _userItems)
        {
            //item.ToggleLoadingObject(true);
            if (!item.IsBank)
            {
                item.EndBet();
            }
        }
    }

    private void OnPlayerDo(ISFSObject sfsObj) //this will receive when a player do
    {
        string[] playerCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        int modifier = sfsObj.GetInt(GameConstants.MODIFIER);
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);

        Debug.Log($"{playerName} is DO with Cards [{playerCards[0]}, {playerCards[1]}] and total value of {totalValue}");

        GetUserItemByName(playerName).SetTotalValue(totalValue);
        GetUserItemByName(playerName).PlayerDo(totalValue);
        GetUserItemByName(playerName).UpdateAllCards(playerCards);
        GetUserItemByName(playerName).SetModifier(modifier);

        if (playerName == GlobalManager.Instance.GetSfsClient().MySelf.Name || GetUserItemByName(playerName).IsBank)
        {
            //CardViewPanel.Instance.ClosePanel();
        }

        if (playerName == GlobalManager.Instance.GetSfsClient().MySelf.Name)
        {
            
        }
    }

    private void OnDrawCard(ISFSObject sfsObj) //this will receive when a player hit
    {
        Managers.AudioManager.PlayDrawCardClip();
        string drawerName = sfsObj.GetUtfString(GameConstants.USER_NAME);
        CardAnimationController contrlr = GetComponent<CardAnimationController>();

        GetUserItemByName(drawerName).EndTurn();

        if (drawerName == Managers.NetworkManager.SmartFox.MySelf.Name)
        {
            ToggleGameplayBtns(false);
            string drawnCardName = sfsObj.GetUtfString(GameConstants.CARD_NAME);
            int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
            int modifier = sfsObj.GetInt(GameConstants.MODIFIER);
            Debug.Log($"{GetUserItemByName(drawerName).Name} draw Card [{drawnCardName}] and total value is {totalValue}");
            //GetUserItemByName(drawerName).SetTotalValue(totalValue);
            //GetUserItemByName(drawerName).AddCard(drawnCardName);
            
            contrlr.DistributeCardToSinglePlayer(drawnCardName, totalValue, modifier, GetUserItemByName(drawerName));
        }
        else
        {
            Debug.Log($"{drawerName} draw a card ..");
            //GetUserItemByName(drawerName).AddBlankCards();
            contrlr.DistributeBlankCardToSinglePlayer(GetUserItemByName(drawerName));
        }
    }

    private void OnPlayerHandCard(ISFSObject sfsObj) //this will receive when server send u ur own hand cards
    {
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);

        string[] handCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);

        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        int modifier = sfsObj.GetInt(GameConstants.MODIFIER);
        bool isDo = sfsObj.GetBool(GameConstants.IS_DO);

        GetUserItemByName(playerName).SetTotalValue(totalValue);
        GetUserItemByName(playerName).SetModifier(modifier);
        GetUserItemByName(playerName).UpdateAllCards(handCards);

        if (isDo)
        {
            //GetUserItemByName(playerName).PlayerDo(totalValue);
        }

        if (playerName == GlobalManager.Instance.GetSfsClient().MySelf.Name)
        {
            CardViewPanel.Instance.SetTwoCardsAndShow(handCards[0], handCards[1], GetUserItemByName(playerName), isDo, totalValue);
        }
    }

    private void OnWinEvent(ISFSObject sfsObj) //this will receive when a player win
    {
        foreach (RoomUserItem item in _userItems)
        {
            item.EndTurn();
            item.ShowCards();
        }

        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);
        string[] handCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        int totalAmount = sfsObj.GetInt(GameConstants.TOTAL_AMOUNT);
        int bankAmount = sfsObj.GetInt(GameConstants.BANK_AMOUNT);
        int modifier = sfsObj.GetInt(GameConstants.MODIFIER);
        int amountChanged = sfsObj.GetInt(GameConstants.AMOUNT_CHANGED);
        bool isDo = sfsObj.GetBool(GameConstants.IS_DO);

        if (isDo)
        {
            GetUserItemByName(playerName).PlayerDo(totalValue);
        }

        Debug.Log("Bank : " + bankAmount);
        _bankAmountTxt.text = bankAmount.ToString();

        _bankDisplay.DisplayNumber(bankAmount.ToString());

        GetUserItemByName(playerName).UpdateAllCards(handCards);

        GetUserItemByName(playerName).SetTotalValue(totalValue);
        GetUserItemByName(playerName).SetAmount(totalAmount);
        GetUserItemByName(playerName).SetModifier(modifier);
        GetUserItemByName(playerName).WinLose(true, amountChanged);
        ToggleGameplayBtns(false);
    }

    private void OnLoseEvent(ISFSObject sfsObj) //this will receive when a player lose
    {
        foreach (RoomUserItem item in _userItems)
        {
            item.EndTurn();
            item.ShowCards();

        }

        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);
        string[] handCards = sfsObj.GetUtfStringArray(GameConstants.PLAYER_CARD_ARRAY);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        int totalAmount = sfsObj.GetInt(GameConstants.TOTAL_AMOUNT);
        int bankAmount = sfsObj.GetInt(GameConstants.BANK_AMOUNT);
        int modifier = sfsObj.GetInt(GameConstants.MODIFIER);
        int amountChanged = sfsObj.GetInt(GameConstants.AMOUNT_CHANGED);
        bool isDo = sfsObj.GetBool(GameConstants.IS_DO);

        if (isDo)
        {
            GetUserItemByName(playerName).PlayerDo(totalValue);
        }

        Debug.Log("Bank : " + bankAmount);
        _bankAmountTxt.text = bankAmount.ToString();

        _bankDisplay.DisplayNumber(bankAmount.ToString());

        foreach (string handCard in handCards)
        {
            Debug.Log(playerName + " " + handCard);
        }

        GetUserItemByName(playerName).UpdateAllCards(handCards);

        GetUserItemByName(playerName).SetTotalValue(totalValue);
        GetUserItemByName(playerName).SetAmount(totalAmount);
        GetUserItemByName(playerName).SetModifier(modifier);
        GetUserItemByName(playerName).WinLose(false, amountChanged);
        ToggleGameplayBtns(false);
    }

    private void OnPlayerTotalValue(ISFSObject sfsObj)
    {
        string playerName = sfsObj.GetUtfString(GameConstants.USER_NAME);
        int totalValue = sfsObj.GetInt(GameConstants.TOTAL_VALUE);
        //Debug.Log("Player Total Value : " + totalValue);
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
        _pleaseWaitTxt.SetActive(false);
        ToggleGameplayBtns(false);

        foreach (RoomUserItem item in _userItems)
        {
            item.Reset();
        }
    }
}
