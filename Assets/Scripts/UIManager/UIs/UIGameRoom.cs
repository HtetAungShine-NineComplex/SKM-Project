using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameRoom : UiBase
{
    [SerializeField] private GameplayManagerShan _gameManager;
    [SerializeField] private Button settingBtn;
    [SerializeField] private GameObject settingPopup;

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);

        _gameManager.Initialize();
        settingBtn.onClick.AddListener(toggleSettingPopup);
    }

    public override void OnClose()
    {
        base.OnClose();

        _gameManager.RemovenServerEvents();
    }

    private void toggleSettingPopup()
    {
        if(settingPopup != null)
        {
            bool isActive = settingPopup.activeSelf;
            settingPopup.SetActive(!isActive);
        }
    }
}
