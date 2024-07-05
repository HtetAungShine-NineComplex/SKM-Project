using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UiBase
{
    [SerializeField] private Button _lobbyBtn; //sample

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);

        _lobbyBtn.onClick.AddListener(ToLobby);
    }

    public override void OnClose()
    {
        base.OnClose();
        _lobbyBtn.onClick.RemoveAllListeners();
    }

    private void ToLobby()
    {
        Managers.UIManager.ShowUI(UIs.UIRoom);
    }
}
