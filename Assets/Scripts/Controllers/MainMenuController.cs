using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using THZ;
using UnityEngine;

public class MainMenuController : BaseSceneController
{
    private SmartFox sfs;

    protected override void HideModals()
    {
        
    }

    public void RequestJoinRoom()
    {
        Debug.Log("Sfs is connected : " + sfs.IsConnected);
        if (!sfs.IsConnected)
        {
            
            return;
        }

        SFSObject data = new SFSObject();
        data.PutInt(GameConstants.REQUEST_ROOM_AMOUNT, 1000);
        GlobalManager.Instance.GetSfsClient().Send(new ExtensionRequest("joinRoom", data));
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
        //warningPanel.Show("Room join failed: " + (string)evt.Params["errorMessage"]);
    }

    public void AddSmartFoxListeners()
    {
        if (sfs == null)
        {
            // Set a reference to the SmartFox client instance
            sfs = gm.GetSfsClient();
        }

        sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);

        // Populate list of available games
        //PopulateGamesList();
    }

    /**
	 * Remove all SmartFoxServer-related event listeners added by the scene.
	 * This method is called by the parent BaseSceneController.OnDestroy method when the scene is destroyed.
	 */
    public override void RemoveSmartFoxListeners()
    {
        if (sfs == null)
        {
            return;
        }
        sfs.RemoveEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        sfs.RemoveEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
    }
}
