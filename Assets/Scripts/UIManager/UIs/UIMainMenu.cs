using Newtonsoft.Json;
using Proyecto26;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Util;
using Shan.API;
using System;
using System.Collections;
using System.Collections.Generic;
using THZ;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UiBase
{
    [SerializeField] private Button _playBtn; //sample
    [SerializeField] private Button _lobbyBtn; //sample
    [SerializeField] private MainMenuController _menuController;
    [Header("UserInfoTexts")]
    [SerializeField] private TMP_Text _usernameTxt;
    [SerializeField] private TMP_Text _balanceTxt;

    [Header("Avator Selection")]
    [SerializeField] private GameObject _UIAvatorSelection;
    [SerializeField] private Button _avatorSelectionBtn; //sample //srat
    [SerializeField] private Sprite _avatorMale;//srat
    [SerializeField] private Sprite _avatorFemale;//srat
    [SerializeField] private bool _choosedMale;//srat
    [SerializeField] private Button MaleBtn;//srat
    [SerializeField] private Button FemaleBtn;//srat

    [Header("Settings")]
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _CloseSettingBtn;
    [SerializeField] private GameObject _UIMainMenuSetting;

    [Header("History")]
    [SerializeField] private Button _historyBtn;
    [SerializeField] private Button _CloseHistoryBtn;
    [SerializeField] private GameObject _UILifeTimeHistory;

    [SerializeField] private GameObject _noTokenPanel;
    [SerializeField] private GameObject _noMoneyPanel;

    private SmartFox sfs;

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);
#if UNITY_WEBGL && !UNITY_EDITOR
        if (Managers.isTokenEmpty)
        {
            _noTokenPanel.SetActive(true);
            return;
        }
        _noTokenPanel.SetActive(false);
#endif

        //Connect();
        GetUserInfo();
        
        Managers.AudioManager.PlayBGMusic();
        _lobbyBtn.onClick.AddListener(ToLobby);
        _avatorSelectionBtn.onClick.AddListener(OpenAvatorSelection);//srat

        MaleBtn.onClick.AddListener(ChooseMale);
        FemaleBtn.onClick.AddListener(ChooseFemale);


        _settingBtn.onClick.AddListener(OpenMainMenuSetting);
        _CloseSettingBtn.onClick.AddListener(CloseMainMenuSetting);

        _historyBtn.onClick.AddListener(OpenLifeTimeHistory);
        _CloseHistoryBtn.onClick.AddListener(CloseLifeTimeHistory);
        UpdateAvator();

        _lobbyBtn.interactable = false;
        _playBtn.interactable = false;
    }

    public void GetUserInfo()
    {
        string uri = Managers.DataLoader.NetworkData.URI + Managers.DataLoader.NetworkData.userInfo;

        RequestHelper currentRequest = new RequestHelper
        {
            Uri = uri,
            Headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + PlayerPrefs.GetString("token") }
            }
          ,
            EnableDebug = true
        };
        RestClient.Get(currentRequest)
        .Then(res => {
            UserInfoResponse r = JsonConvert.DeserializeObject<UserInfoResponse>(res.Text);
            if(r.status == "success")
            {
                _usernameTxt.text = r.data.name;
                Managers.DataLoader.CurrentName = r.data.name;
                Managers.DataLoader.CurrentAmount = r.data.balance;
                Debug.Log("username: " + r.data.name);
                _balanceTxt.text = r.data.balance.ToString();
#if UNITY_WEBGL
				Connect();
#endif
            }
        })
        .Catch(err => Debug.Log(err.Message));
    }

    public void Connect()
    {
        // Set connection parameters
        ConfigData cfg = new ConfigData();
        cfg.Host = Managers.DataLoader.NetworkData.host;

#if UNITY_WEBGL //&& !UNITY_EDITOR

        Debug.Log("Webgl");
        cfg.Port = Managers.DataLoader.NetworkData.webSocketPort;
        //cfg.Port = tcpPort;
#else
        cfg.Port = tcpPort;
#endif
        cfg.UdpHost = Managers.DataLoader.NetworkData.host;
        cfg.UdpPort = Managers.DataLoader.NetworkData.UdpPort;
        cfg.Zone = Managers.DataLoader.NetworkData.zone;
        cfg.Debug = Managers.DataLoader.NetworkData.debug;

#if UNITY_WEBGL //&& !UNITY_EDITOR
        GlobalManager.Instance.CreateSfsClient(UseWebSocket.WSS_BIN);
#else
        sfs = gm.CreateSfsClient();
#endif
        sfs = GlobalManager.Instance.GetSfsClient();

        sfs.Logger.EnableConsoleTrace = Managers.DataLoader.NetworkData.debug;

        AddSmartFoxListeners();
        _menuController.AddSmartFoxListeners();
        Managers.NetworkManager.SubscribeDelegates();

        sfs.Connect(cfg);
    }

    private void AddSmartFoxListeners()
    {
        sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
        sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
    }

    public void RemoveSmartFoxListeners()
    {
        // NOTE
        // If this scene is stopped before a connection is established, the SmartFox client instance
        // could still be null, causing an error when trying to remove its listeners

        if (sfs != null)
        {
            sfs.RemoveEventListener(SFSEvent.CONNECTION, OnConnection);
            sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
            sfs.RemoveEventListener(SFSEvent.LOGIN, OnLogin);
            sfs.RemoveEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        }
    }

    private void OnConnection(BaseEvent evt)
    {
        // Check if the conenction was established or not
        if ((bool)evt.Params["success"])
        {
            Debug.Log("SFS2X API version: " + sfs.Version);
            Debug.Log("Connection mode is: " + sfs.ConnectionMode);

            // Login
            sfs.Send(new LoginRequest(Managers.DataLoader.CurrentName));
        }
        else
        {

        }
    }

    private void OnConnectionLost(BaseEvent evt)
    {
        // Remove SFS listeners
        RemoveSmartFoxListeners();

        // Show error message
        string reason = (string)evt.Params["reason"];

        if (reason != ClientDisconnectionReason.MANUAL)
        {

        }
    }

    private void OnLogin(BaseEvent evt)
    {
#if !UNITY_WEBGL
    // Initialize UDP communication
    sfs.InitUDP();
#else
        // For WebGL, you might want to initialize WebSocket or handle differently
        Debug.Log("WebGL does not support UDP. Proceeding without UDP initialization.");
        //OnUdpInit(new BaseEvent("udpInit", new Dictionary<string, object> { { "success", true } }));
        _lobbyBtn.interactable = true;
        _playBtn.interactable = true;
#endif
    }

    private void OnLoginError(BaseEvent evt)
    {
        // Disconnect
        // NOTE: this causes a CONNECTION_LOST event with reason "manual", which in turn removes all SFS listeners
        sfs.Disconnect();

    }

    private void UpdateAvator() //srat
    {
        if (_choosedMale)
        {
            //change male avator
            _avatorSelectionBtn.image.sprite =_avatorMale;
        }
        else
        {
            //change female avator
            _avatorSelectionBtn.image.sprite =_avatorFemale;
        }
    }

    private void ChooseMale()//srat
    {
        if (_UIAvatorSelection != null)
        {
            bool isActive = _UIAvatorSelection.activeSelf;
            //change avator
            ChooseAvator(true);
            UpdateAvator();
            _UIAvatorSelection.SetActive(!isActive);
            Debug.Log("closed avator selection panel!");
        }
    }

    private void ChooseFemale()//srat
    {
        if (_UIAvatorSelection != null)
        {
            bool isActive = _UIAvatorSelection.activeSelf;
            //change avator
            ChooseAvator(false);
            UpdateAvator() ;
            _UIAvatorSelection.SetActive(!isActive);
            Debug.Log("closed avator selection panel!");
        }
    }

    public void ChooseAvator(bool ismale)//srat
    {
        _choosedMale = ismale;
    }
    private void OpenAvatorSelection() //srat
    {
        if(_UIAvatorSelection!=null)
        {
            _UIAvatorSelection.SetActive(true);
            Debug.Log("opened avator selection panel!");
        }
    }
/////////////////////////////////////////////////////////////////////////////

    public void OpenMainMenuSetting()
    {
        if (_UIMainMenuSetting != null)
        {
            _UIMainMenuSetting.SetActive(true);
            Debug.Log("opened mainmenu setting panel!");
        }
    }

    public void CloseMainMenuSetting()
    {
        if (_UIMainMenuSetting != null)
        {
            _UIMainMenuSetting.SetActive(false);
            Debug.Log("closed mainmenu setting panel!");
        }
    }
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void OpenLifeTimeHistory()
    {
        if (_UILifeTimeHistory != null)
        {
            _UILifeTimeHistory.SetActive(true);
            Debug.Log("opened mainmenu setting panel!");
        }
    }

    public void CloseLifeTimeHistory()
    {
        if (_UILifeTimeHistory != null)
        {
            _UILifeTimeHistory.SetActive(false);
            Debug.Log("closed mainmenu setting panel!");
        }
    }
    /// ///////////////////////////////////////////////////////////////////////////


    public override void OnClose()
    {
        base.OnClose();
        _lobbyBtn.onClick.RemoveAllListeners();
        _avatorSelectionBtn.onClick.RemoveAllListeners();//srat
        _menuController.RemoveSmartFoxListeners();
    }

    private void ToLobby()
    {
        Managers.UIManager.ShowUI(UIs.UIRoom);
    }

    public void ReqJoinRoom()
    {
        if(Managers.DataLoader.CurrentAmount <= 0)
        {
            _noMoneyPanel.SetActive(true);
            return;
        }
        _menuController.RequestJoinRoom();
    }

}
