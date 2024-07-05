using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private CardDataManagerDemo _cardDataManager;

    [HideInInspector] public static UIManager UIManager;
    [HideInInspector] public static NetworkManager NetworkManager;
    [HideInInspector] public static CardDataManagerDemo CardDataManager;

    private void Awake()
    {
        UIManager = _uiManager;
        NetworkManager = _networkManager;
        CardDataManager = _cardDataManager;

        UIManager.Initialize();
        
        ShowInitialUI();
    }

    void ShowInitialUI()
    {
        UIManager.ShowUI(UIs.UILogin);
    }
}
