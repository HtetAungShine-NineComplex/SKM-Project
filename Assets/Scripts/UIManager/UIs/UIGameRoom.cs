using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameRoom : UiBase
{
    [SerializeField] private GameplayManagerShan _gameManager;
    [SerializeField] private CardAnimationController _cardAnimController;
    [SerializeField] private Button settingBtn;
    [SerializeField] private GameObject settingPopup;

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);

        _gameManager.Initialize();
        //
        _cardAnimController.ListenEvents();
        settingBtn.onClick.AddListener(toggleSettingPopup);
        Managers.AudioManager.StopBGMusic();

    }

    public override void OnClose()
    {
        base.OnClose();

        _gameManager.RemovenServerEvents();
        _cardAnimController.RemoveEvents();
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
