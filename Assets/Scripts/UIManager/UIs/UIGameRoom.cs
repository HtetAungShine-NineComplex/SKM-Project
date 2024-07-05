using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameRoom : UiBase
{
    [SerializeField] private GameplayManagerShan _gameManager;

    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);

        _gameManager.Initialize();
    }

    public override void OnClose()
    {
        base.OnClose();

        _gameManager.RemovenServerEvents();
    }
}
