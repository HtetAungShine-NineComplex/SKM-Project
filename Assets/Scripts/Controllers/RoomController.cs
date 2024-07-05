using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomController : BaseSceneController
{
    
    [SerializeField] private GameplayManagerDemo _gameManager;

    private SmartFox sfs;

    private void Start()
    {
        sfs = gm.GetSfsClient();

        _gameManager.Initialize();

    }

    protected override void HideModals()
    {
        
    }

    public override void RemoveSmartFoxListeners()
    {
        
    }

    

    
}
