using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingScreen : UiBase
{
    [SerializeField] private GameObject loadingBar;
    [SerializeField] private int desiredTime;

   public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);
        animateLoadingBar();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
    private void animateLoadingBar()
    {
        LeanTween.scaleX(loadingBar,1,desiredTime).setOnComplete(showMainMenuUI);
    }

    private void showMainMenuUI()
    {
        Managers.UIManager.ShowUI(UIs.UIMainMenu);
    }
}
