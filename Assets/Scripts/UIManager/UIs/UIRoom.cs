using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoom : UiBase
{
    [SerializeField] private LobbyController _lobbyCtrlr;

    [SerializeField] private Button _BackBtn;//back to main menu //srat

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);
        _BackBtn.onClick.AddListener(BackBtn);
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
