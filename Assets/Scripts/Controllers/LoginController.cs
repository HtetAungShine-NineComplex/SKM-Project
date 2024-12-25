using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Util;
using TMPro;
using THZ;
using System.Collections.Generic;
using Proyecto26;
using Newtonsoft.Json;
using Shan.API;

/**
 * Script attached to the Controller object in the Login scene.
 */
public class LoginController : BaseSceneController
{
	//----------------------------------------------------------
	// Editor public properties
	//----------------------------------------------------------

	/*[Tooltip("IP address or domain name of the SmartFoxServer instance")]
	public string host = "127.0.0.1";

	[Tooltip("TCP listening port of the SmartFoxServer instance, used for TCP socket connection")]
	public int tcpPort = 9933;

    [Tooltip("UDP listening port of the SmartFoxServer instance, used for UDP communication")]
    public int UdpPort = 9933;

    public int webSocketPort = 8080;

    [Tooltip("Name of the SmartFoxServer Zone to join")]
	public string zone = "DelightShan";

	[Tooltip("Display SmartFoxServer client debug messages")]
	public bool debug = false;*/

	//----------------------------------------------------------
	// UI elements
	//----------------------------------------------------------

	[SerializeField] private TMP_InputField nameInput;
	[SerializeField] private TMP_InputField pwdInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text errorText;

	//----------------------------------------------------------
	// Private properties
	//----------------------------------------------------------

	private SmartFox sfs;

	//----------------------------------------------------------
	// Unity calback methods
	//----------------------------------------------------------

	private void Start()
	{
		// Focus on username input
		nameInput.Select();
		nameInput.ActivateInputField();

		// Show connection lost message, in case the disconnection occurred in another scene
		string connLostMsg = gm.ConnectionLostMessage;
		if (connLostMsg != null)
			errorText.text = connLostMsg;
	}

	//----------------------------------------------------------
	// UI event listeners
	//----------------------------------------------------------
	#region
	/**
	 * On username input edit end, if the Enter key was pressed, connect to SmartFoxServer.
	 */
	public void OnNameInputEndEdit()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			Connect();
	}

	/**
	 * On Login button click, connect to SmartFoxServer.
	 */
	public void OnLoginButtonClick()
	{
		//Connect();
		

		Login(nameInput.text, pwdInput.text);
	}

	public void Login(string phone, string password) //post
	{
		errorText.text = "";
		EnableUI(false);

		APILoginRequest reqData = new APILoginRequest(phone, password);
		string uri = Managers.DataLoader.NetworkData.URI + Managers.DataLoader.NetworkData.login;


        RequestHelper currentRequest = new RequestHelper
		{
			Uri = uri
			,
			BodyString = JsonConvert.SerializeObject(reqData)
			,
			EnableDebug = true
		};
		RestClient.Post(currentRequest)
		.Then(res =>
		{
			EnableUI(true);

			LoginResponse r = JsonConvert.DeserializeObject<LoginResponse>(res.Text);

			if (r.status == "success")
			{
                PlayerPrefs.SetString("token", r.data.token);
				Managers.DataLoader.CurrentName = r.data.user.name;

                //PlayerPrefs.Save();
#if !UNITY_WEBGL
				Connect();
#endif
                //Connect();
                Debug.Log(" token : " + r.data.token);
            }
			else
			{
				errorText.text = r.message;
			}
		})
		.Catch(err => { 
			Debug.Log(err.Message);
            EnableUI(true);
            errorText.text = "Incorrect Username or Password";
        });
	}
	#endregion

	//----------------------------------------------------------
	// Helper methods
	//----------------------------------------------------------
	#region
	/**
	 * Enable/disable username input interaction.
	 */
	private void EnableUI(bool enable)
	{
		nameInput.interactable = enable;
		pwdInput.interactable = enable;
		loginButton.interactable = enable;
    }

	/**
	 * Connect to SmartFoxServer.
	 */
	public void Connect()
	{
		// Disable user interface
		EnableUI(false);

		// Clear any previour error message
		errorText.text = "";

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
        sfs = gm.CreateSfsClient(UseWebSocket.WS_BIN);
#else
        sfs = gm.CreateSfsClient();
#endif
        sfs.Logger.EnableConsoleTrace = Managers.DataLoader.NetworkData.debug;

		AddSmartFoxListeners();
		Managers.NetworkManager.SubscribeDelegates();

		sfs.Connect(cfg);
	}

	/**
	 * Add all SmartFoxServer-related event listeners required by the scene.
	 */
	private void AddSmartFoxListeners()
	{
		sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
		sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        sfs.AddEventListener(SFSEvent.UDP_INIT, OnUdpInit);
    }

	/**
	 * Remove all SmartFoxServer-related event listeners added by the scene.
	 * This method is called by the parent BaseSceneController.OnDestroy method when the scene is destroyed.
	 */
	override public void RemoveSmartFoxListeners()
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
            sfs.RemoveEventListener(SFSEvent.UDP_INIT, OnUdpInit);
        }
	}

	/**
	 * Hide all modal panels.
	 */
	override protected void HideModals()
	{
		// No modals used by this scene
	}
	#endregion

	//----------------------------------------------------------
	// SmartFoxServer event listeners
	//----------------------------------------------------------
	#region
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
			// Show error message
			errorText.text = "Connection failed; is the server running at all?";

			// Enable user interface
			EnableUI(true);
		}
	}

	private void OnConnectionLost(BaseEvent evt)
	{
		// Remove SFS listeners
		RemoveSmartFoxListeners();

		// Show error message
		string reason = (string)evt.Params["reason"];
		
		if (reason != ClientDisconnectionReason.MANUAL)
			errorText.text = "Connection lost; reason is: " + reason;

		// Enable user interface
		EnableUI(true);
    }

    private void OnLogin(BaseEvent evt)
    {
#if !UNITY_WEBGL
    // Initialize UDP communication
    sfs.InitUDP();
#else
        // For WebGL, you might want to initialize WebSocket or handle differently
        Debug.Log("WebGL does not support UDP. Proceeding without UDP initialization.");
        OnUdpInit(new BaseEvent("udpInit", new Dictionary<string, object> { { "success", true } }));
#endif
    }

    private void OnLoginError(BaseEvent evt)
	{
		// Disconnect
		// NOTE: this causes a CONNECTION_LOST event with reason "manual", which in turn removes all SFS listeners
		sfs.Disconnect();

		// Show error message
		errorText.text = "Login failed due to the following error:\n" + (string)evt.Params["errorMessage"];

		// Enable user interface
		EnableUI(true);
	}

	private void OnUdpInit(BaseEvent evt)
	{
		if ((bool)evt.Params["success"])
		{
			// Set invert mouse Y option
			//	OptionsManager.InvertMouseY = invertMouseToggle.isOn;
			//SF2X_GameManager.invertMouseY = invertMouseToggle.isOn;

			// Load lobby scene
			//SceneManager.LoadScene(GameConstants.LOBBY_SCENE) ;
			Managers.UIManager.ShowUI(UIs.UILoadingScreen);
		}
		else
		{
			// Disconnect
			// NOTE: this causes a CONNECTION_LOST event with reason "manual", which in turn removes all SFS listeners
			sfs.Disconnect();

			// Show error message
			errorText.text = "UDP initialization failed due to the following error:\n" + (string)evt.Params["errorMessage"];

			// Enable user interface
			EnableUI(true);
		}
	}
    #endregion
}
