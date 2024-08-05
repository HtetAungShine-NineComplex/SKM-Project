using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoom : UiBase
{
    [SerializeField] private LobbyController _lobbyCtrlr;

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);

        _lobbyCtrlr.AddSmartFoxListeners();
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
}
