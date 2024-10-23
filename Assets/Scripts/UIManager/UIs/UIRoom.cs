using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using Shan.API;
using Newtonsoft.Json;
using Sfs2X;

public class UIRoom : UiBase
{
    [SerializeField] private LobbyController _lobbyCtrlr;

    [SerializeField] private Button _BackBtn;//back to main menu //srat

    // A list to store RoomConfig objects
    [SerializeField] private List<RoomConfigData> savedRoomConfigs = new List<RoomConfigData>();

    public Transform gameListContent;
    public GameListItem gameListItemPrefab;

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);
        _BackBtn.onClick.AddListener(BackBtn);
        _lobbyCtrlr.AddSmartFoxListeners();
        GetRoomConfig();
    }

    public override void OnClose()
    {
        base.OnClose();

        _lobbyCtrlr.RemoveSmartFoxListeners();
    }


    public void BackBtn()
    {
        Managers.UIManager.ShowUI(UIs.UIMainMenu);
    }

    public void GetRoomConfig()
    {
        string uri = Managers.DataLoader.NetworkData.URI + Managers.DataLoader.NetworkData.roomconfig;

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
            RoomConfigResponse r = JsonConvert.DeserializeObject<RoomConfigResponse>(res.Text);
            if (r.status == "success")
            {
                // Check how many room configurations were received
                int numOfRoomConfigs = r.data.Count;
                Debug.Log("Number of Room Configurations: " + numOfRoomConfigs);

                // Save each room config in the savedRoomConfigs list
                foreach (RoomConfigData roomConfig in r.data)
                {
                    // Save room config to the list
                    savedRoomConfigs.Add(roomConfig);

                    // Log individual room details (optional)
                    Debug.Log("Room ID: " + roomConfig.id);
                    Debug.Log("Room Name: " + roomConfig.name);
                    Debug.Log("Min Amount: " + roomConfig.min_amount);
                    Debug.Log("Max Amount: " + roomConfig.max_amount);
                    Debug.Log("Bet Amounts: " + string.Join(", ", roomConfig.bet_amounts));
                }
            }
            else
            {
                Debug.LogError("RoomConfig Response is failed to fetch room configs.");
            }
        })
        .Catch(err => Debug.LogError("RoomConfig Response=>" + err.Message));
    }
}
