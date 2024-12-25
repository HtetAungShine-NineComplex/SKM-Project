using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogin : UiBase
{
    [SerializeField] private LoginController _loginCtrlr;

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);
    }

    public override void OnClose()
    {
        base.OnClose();

        _loginCtrlr.RemoveSmartFoxListeners();
    }

    
}
