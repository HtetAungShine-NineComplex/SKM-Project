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

//TODO refactor for different types of game rooms
public class NetworkManager : MonoBehaviour
{
    private SmartFox _smartfox;


    [SerializeField] private TMP_Text _pingText;
    [SerializeField] private GameObject _gameStartObj;

    public SmartFox SmartFox => _smartfox;

    //game events
    public event Action<ISFSObject> BotJoined;
    public event Action<ISFSObject> BetStarted;
    public event Action<ISFSObject> GameStarted;
    public event Action<ISFSObject> Countdown;
    public event Action<ISFSObject> StartCurrentTurn;
    public event Action<ISFSObject> PlayerHit;
    public event Action<ISFSObject> PlayerWin;
    public event Action<ISFSObject> PlayerLose;
    public event Action<ISFSObject> PlayerTotalValue;
    public event Action<ISFSObject> Owner;
    public event Action<ISFSObject> Banker;
    public event Action<ISFSObject> PlayerDo;
    public event Action<ISFSObject> PlayerDrawCard;
    public event Action<ISFSObject> PlayerHandCards;
    public event Action<ISFSObject> RoomPlayerList;
    public event Action<ISFSObject> PlayerBet;
    public event Action<ISFSObject> PleaseWait;
    public event Action<ISFSObject> MatchEnd;

    //server events
    public event Action<User> UserEnterRoom;
    public event Action<User> UserLeaveRoom;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {


        SubscribeDelegates();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region  SmartFoxServer Event Listeners

    public void SubscribeDelegates()
    {
        _smartfox = GlobalManager.Instance.GetSfsClient();

        _smartfox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        _smartfox.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserLeaveRoom);
        _smartfox.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserJoinRoom);
        _smartfox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        //_smartfox.AddEventListener(SFSEvent.PING_PONG, OnPingPong);
    }

    private void UnsubscribeDelegates()
    {
        _smartfox.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        _smartfox.RemoveEventListener(SFSEvent.USER_EXIT_ROOM, OnUserLeaveRoom);
        _smartfox.RemoveEventListener(SFSEvent.USER_ENTER_ROOM, OnUserJoinRoom);
        _smartfox.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
    }

    private void OnConnectionLost(BaseEvent evt)
    {
        UnsubscribeDelegates();
        SceneManager.LoadScene("Login");
    }

    public void OnExitGame()
    {
        UnsubscribeDelegates();
        _smartfox.Send(new LeaveRoomRequest());
        SceneManager.LoadScene("Login");
    }

    private void OnUserJoinRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        Room room = (Room)evt.Params["room"];
        string info = (user.Name + " has joined the game");
        Debug.Log(info);
        UserEnterRoom?.Invoke(user);
    }

    private void OnUserLeaveRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        Room room = (Room)evt.Params["room"];
        string info = (user.Name + " has left the game");

        Debug.Log(info);
        UserLeaveRoom?.Invoke(user);
    }

    void OnApplicationQuit()
    {
        UnsubscribeDelegates();
    }

    public void OnPingPong(BaseEvent evt)
    {
        int ping = (int)evt.Params["lagValue"] / 2;

        Debug.Log("Ping is " + ping + "ms");
        //_pingText.text = "Ping: " + ping + "ms";
    }

    #endregion


    public void SendRequest(ExtensionRequest request)
    {
        if (request != null && _smartfox != null && _smartfox.IsConnected)
            _smartfox.Send(request);
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        string cmd = (string)evt.Params["cmd"];
        ISFSObject sfsobject = (SFSObject)evt.Params["params"];
        switch (cmd)
        {
            case GameConstants.BOT_JOINED:
                OnBotJoined(sfsobject);
                break;

            case GameConstants.GAME_STARTED:
                OnGameStarted(sfsobject);
                break;

            case GameConstants.BET_STARTED:
                OnBetStarted(sfsobject);
                break;

            case GameConstants.COUNTDOWN:
                OnCountdown(sfsobject);
                break;

            case GameConstants.OWNER:
                OnOwner(sfsobject);
                break;

            case GameConstants.BANKER:
                OnBanker(sfsobject);
                break;

            case GameConstants.START_CURRENT_TURN:
                OnStartCurrentTurn(sfsobject);
                break;

            case GameConstants.PLAYER_HIT:
                OnPlayerHit(sfsobject);
                break;

            case GameConstants.PLAYER_WIN:
                OnPlayerWin(sfsobject);
                break;

            case GameConstants.PLAYER_LOSE:
                OnPlayerLose(sfsobject);
                break;

            case GameConstants.PLAYER_DO:
                OnPlayerDo(sfsobject);
                break;

            case GameConstants.PLAYER_DRAW:
                OnPlayerDraw(sfsobject);
                break;

            case GameConstants.PLAYER_HAND_CARDS:
                OnPlayerHandCards(sfsobject);
                break;

            case GameConstants.ROOM_PLAYER_LIST:
                OnRoomPlayerList(sfsobject);
                break;

            case GameConstants.PLAYER_BET:
                OnPlayerBet(sfsobject);
                break;

            case GameConstants.PLEASE_WAIT:
                OnPleaseWait(sfsobject);
                break;
            
            case GameConstants.MATCH_END:
                MatchEnd(sfsobject);
                break;
        }
    }

    private void OnBotJoined(ISFSObject sfsObj)
    {
        BotJoined?.Invoke(sfsObj);
    }

    private void OnGameStarted(ISFSObject sfsObj)
    {
        Debug.Log("Game Started");
        GameStarted?.Invoke(sfsObj);
    }

    private void OnBetStarted(ISFSObject sfsObj)
    {
        Debug.Log("Bet Started");
        BetStarted?.Invoke(sfsObj);
    }

    private void OnCountdown(ISFSObject sfsObj)
    {
        int countdown = sfsObj.GetInt("countdown");
        //Debug.Log("Game will start in: " + countdown);

        Countdown?.Invoke(sfsObj);
    }

    private void OnOwner(ISFSObject sfsObj)
    {
        Owner?.Invoke(sfsObj);
    }

    private void OnBanker(ISFSObject sfsObj)
    {
        Banker?.Invoke(sfsObj);
    }

    private void OnStartCurrentTurn(ISFSObject sfsObj)
    {
        StartCurrentTurn?.Invoke(sfsObj);
    }

    public void OnPlayerHit(ISFSObject sfsObj)
    {
        PlayerHit?.Invoke(sfsObj);
    }

    public void OnPlayerWin(ISFSObject sfsObj)
    {
        PlayerWin?.Invoke(sfsObj);
    }

    public void OnPlayerLose(ISFSObject sfsObj)
    {
        PlayerLose?.Invoke(sfsObj);
    }

    public void OnPlayerTotalValue(ISFSObject sfsObj)
    {
        PlayerTotalValue?.Invoke(sfsObj);
    }

    public void OnPlayerDo(ISFSObject sfsObj)
    {
        PlayerDo?.Invoke(sfsObj);
    }

    public void OnPlayerDraw(ISFSObject sfsObj)
    {
        PlayerDrawCard?.Invoke(sfsObj);
    }

    public void OnPlayerHandCards(ISFSObject sfsObj)
    {
        PlayerHandCards?.Invoke(sfsObj);
    }

    public void OnRoomPlayerList(ISFSObject sfsObj)
    {
        RoomPlayerList?.Invoke(sfsObj);
    }

    public void OnPlayerBet(ISFSObject sfsObj)
    {
        PlayerBet?.Invoke(sfsObj);
    }

    public void OnPleaseWait(ISFSObject sfsObj)
    {
        PleaseWait?.Invoke(sfsObj);
    }

    public void OnMatchEnd(ISFSObject sfsObj)
    {
        MatchEnd?.Invoke(sfsObj);
    }
}