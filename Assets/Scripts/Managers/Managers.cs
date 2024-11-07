using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private CardDataManagerDemo _cardDataManager;
    [SerializeField] private DataLoader _dataLoader;
    [SerializeField] private AudioManager _audioManager;

    [HideInInspector] public static UIManager UIManager;
    [HideInInspector] public static NetworkManager NetworkManager;
    [HideInInspector] public static CardDataManagerDemo CardDataManager;
    [HideInInspector] public static DataLoader DataLoader;
    [HideInInspector] public static AudioManager AudioManager;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        UIManager = _uiManager;
        NetworkManager = _networkManager;
        CardDataManager = _cardDataManager;
        DataLoader = _dataLoader;
        AudioManager = _audioManager;

        UIManager.Initialize();
        ShowInitialUI();
        AudioManager.Initialize();
    }

    void ShowInitialUI()
    {
        UIManager.ShowUI(UIs.UILogin);
    }
}
