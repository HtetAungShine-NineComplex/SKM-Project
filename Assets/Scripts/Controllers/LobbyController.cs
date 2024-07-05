using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Requests;
using THZ;
using TMPro;

/**
 * Script attached to the Controller object in the Lobby scene.
 */
public class LobbyController : BaseSceneController
{

    //----------------------------------------------------------
    // UI elements
    //----------------------------------------------------------
	public WarningPanel warningPanel;

	public Transform gameListContent;
	public GameListItem gameListItemPrefab;

	//----------------------------------------------------------
	// Private properties
	//----------------------------------------------------------

	private SmartFox sfs;
	private Dictionary<int, GameListItem> gameListItems;

	//----------------------------------------------------------
	// Unity calback methods
	//----------------------------------------------------------

	private void Start()
	{
		

		// Hide modal panels
		//HideModals();

		// Display username in footer
		//sloggedInAsLabel.text = "Logged in as <b>" + sfs.MySelf.Name + "</b>";

		// Add event listeners
		//AddSmartFoxListeners();

		
	}

	//----------------------------------------------------------
	// UI event listeners
	//----------------------------------------------------------
	#region
	/**
	 * On Logout button click, disconnect from SmartFoxServer.
	 * This causes the SmartFox listeners added by this scene to be removed (see BaseSceneController.OnDestroy method)
	 * and the Login scene to be loaded (see GlobalManager.OnConnectionLost method).
	 */
	public void OnLogoutButtonClick()
	{
		// Disconnect from SmartFoxServer
		sfs.Disconnect();
	}

	/**
	 * On Start game button click, create and join a new game Room.
	 */
	public void OnCreateRoomButtonClick()
	{
		// Configure Room
		Debug.Log(sfs.MySelf.Name + " is creating room");
		RoomSettings settings = new RoomSettings(sfs.MySelf.Name + "'s Room");
		settings.GroupId = GameConstants.GAME_ROOMS_GROUP_NAME;
		settings.IsGame = true;
        settings.MaxUsers = GameConstants.GAME_ROOMS_MAX_USER;
        settings.MaxSpectators = 0;
        //settings.Extension = new RoomExtension(GameConstants.EXTENSION_ID, GameConstants.EXTENSION_CLASS);
        settings.Extension = new RoomExtension(GameConstants.SHAN_EXTENSION_ID, GameConstants.SHAN_EXTENSION_CLASS);
        // Request Room creation to server
        sfs.Send(new CreateRoomRequest(settings, true, sfs.LastJoinedRoom));
    }

	/**
	 * On Play game button click in Game List Item prefab instance, join an existing game Room as a player.
	 */
	public void JoinRoomByID(int roomId)
	{
		// Join game Room as player
		sfs.Send(new Sfs2X.Requests.JoinRoomRequest(roomId));
	}
	#endregion

	//----------------------------------------------------------
	// Helper methods
	//----------------------------------------------------------
	#region
	/**
	 * Add all SmartFoxServer-related event listeners required by the scene.
	 */
	public void AddSmartFoxListeners()
	{
		if(sfs == null)
		{
            // Set a reference to the SmartFox client instance
            sfs = gm.GetSfsClient();
        }

		sfs.AddEventListener(SFSEvent.ROOM_CREATION_ERROR, OnRoomCreationError);
		sfs.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdded);
		sfs.AddEventListener(SFSEvent.ROOM_REMOVE, OnRoomRemoved);
		sfs.AddEventListener(SFSEvent.USER_COUNT_CHANGE, OnUserCountChanged);
		sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
		sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);

        // Populate list of available games
        PopulateGamesList();
    }

	/**
	 * Remove all SmartFoxServer-related event listeners added by the scene.
	 * This method is called by the parent BaseSceneController.OnDestroy method when the scene is destroyed.
	 */
	override public void RemoveSmartFoxListeners()
	{
		if(sfs == null)
		{
			return;
		}

		sfs.RemoveEventListener(SFSEvent.ROOM_CREATION_ERROR, OnRoomCreationError);
		sfs.RemoveEventListener(SFSEvent.ROOM_ADD, OnRoomAdded);
		sfs.RemoveEventListener(SFSEvent.ROOM_REMOVE, OnRoomRemoved);
		sfs.RemoveEventListener(SFSEvent.USER_COUNT_CHANGE, OnUserCountChanged);
		sfs.RemoveEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
		sfs.RemoveEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
	}

	/**
	 * Hide all modal panels.
	 */
	override protected void HideModals()
	{
		warningPanel.Hide();
	}

	/**
	 * Display list of existing games.
	 */
	private void PopulateGamesList()
	{
		// Initialize list
		if (gameListItems == null)
			gameListItems = new Dictionary<int, GameListItem>();

		// For the game list we use a scrollable area containing a separate prefab for each Game Room
		// The prefab contains clickable buttons to join the game
		List<Room> rooms = sfs.RoomManager.GetRoomList();

		// Display game list items
		foreach (Room room in rooms)
			AddGameListItem(room);
	}

	/**
	 * Create Game List Item prefab instance and add to games list.
	 */
	private void AddGameListItem(Room room)
	{
		// Show only game rooms
		// Also password protected Rooms are skipped, to make this example simpler
		// (protection would require an interface element to input the password)
		if (!room.IsGame || room.IsHidden || room.IsPasswordProtected)
			return;

		// Create game list item
		GameListItem gameListItem = Instantiate(gameListItemPrefab);
		gameListItems.Add(room.Id, gameListItem);

		// Init game list item
		gameListItem.Init(room);

		// Add listener to play button
		gameListItem.joinBtn.onClick.AddListener(() => JoinRoomByID(room.Id));

		// Add game list item to container
		gameListItem.gameObject.transform.SetParent(gameListContent, false);
	}
	#endregion

	//----------------------------------------------------------
	// SmartFoxServer event listeners
	//----------------------------------------------------------
	#region
	private void OnRoomCreationError(BaseEvent evt)
	{
		// Show Warning Panel prefab instance
		warningPanel.Show("Room creation failed: " + (string)evt.Params["errorMessage"]);
	}

	private void OnRoomAdded(BaseEvent evt)
	{
		Room room = (Room)evt.Params["room"];

		// Display game list item
		AddGameListItem(room);
	}

	public void OnRoomRemoved(BaseEvent evt)
	{
		Room room = (Room)evt.Params["room"];

		// Get reference to game list item corresponding to Room
		gameListItems.TryGetValue(room.Id, out GameListItem gameListItem);

		// Remove game list item
		if (gameListItem != null)
		{
			// Remove listeners
			gameListItem.joinBtn.onClick.RemoveAllListeners();

			// Remove game list item from dictionary
			gameListItems.Remove(room.Id);

			// Destroy game object
			GameObject.Destroy(gameListItem.gameObject);
		}
	}

	public void OnUserCountChanged(BaseEvent evt)
	{
		Room room = (Room)evt.Params["room"];

		// Get reference to game list item corresponding to Room
		gameListItems.TryGetValue(room.Id, out GameListItem gameListItem);

		// Update game list item
		if (gameListItem != null)
			gameListItem.SetState(room);
	}

	private void OnRoomJoin(BaseEvent evt)
	{
		// Load game scene
		//SceneManager.LoadScene(GameConstants.SHAN_ROOM_SCENE);
		Managers.UIManager.ShowUI(UIs.UIGameRoom);
	}

	private void OnRoomJoinError(BaseEvent evt)
	{
		// Show Warning Panel prefab instance
		warningPanel.Show("Room join failed: " + (string)evt.Params["errorMessage"]);
	}
	#endregion
}
