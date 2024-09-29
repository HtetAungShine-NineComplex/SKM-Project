using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAvatorSelection : UiBase
{
    [SerializeField] private GameplayManagerShan _gameManager;

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);

    }

    public override void OnClose()
    {
        base.OnClose();
    }

    

    private void ToMainMenu() //srat
    {
        Managers.UIManager.ShowUI(UIs.UIMainMenu);
    }
}
