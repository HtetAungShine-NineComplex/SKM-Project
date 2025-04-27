using System;
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

    public static bool isTokenEmpty = false;

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
#if UNITY_WEBGL && !UNITY_EDITOR
        GetTokenFromUrl();
        //PlayerPrefs.SetString("token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyX2lkIjoyNiwicGhvbmVfbnVtYmVyIjoiMDkyMjIyMjIyMjIiLCJuYW1lIjoiTmFtZSIsImJhbGFuY2UiOiItODAwNzIuMDAiLCJpYXQiOjE3MzQ0OTY5MDAsImV4cCI6MTczNDU4MzMwMH0.yVF05WY3cuznvGMSZLT2AusLDQrVBQXSZ_9nPX_daKA");
        UIManager.ShowUI(UIs.UIMainMenu);
#else
        UIManager.ShowUI(UIs.UILogin);
#endif

    }

    void GetTokenFromUrl()
    {
        PlayerPrefs.DeleteAll();
        // Get the absolute URL of the current page
        string url = Application.absoluteURL;
        //string url = "https://www.example.com/load_simulation?token=uZVTLBCWcw33RIhvnbxTKxTxM2rKJ7YJrwyUXhXn";

        // Extract the token parameter from the query string
        string token = GetQueryStringParameter(url, "token");

        // Use the token (for example, log it or use it in your game logic)
        Debug.Log("Token: " + token);

        if (string.IsNullOrEmpty(token))
        {
            isTokenEmpty = true;
            Debug.LogError("Token is Empty");
            return;
        }

        PlayerPrefs.SetString("token", token);
        PlayerPrefs.Save();

        //_loginCtrlr.Connect();
        //LoadLoby();
    }

    string GetQueryStringParameter(string url, string key)
    {
        Uri uri = new Uri(url);
        string query = uri.Query;
        var queryParams = ParseQueryString(query);
        queryParams.TryGetValue(key, out string value);
        return value;
    }

    Dictionary<string, string> ParseQueryString(string query)
    {
        var queryDict = new Dictionary<string, string>();
        string[] queryParams = query.TrimStart('?').Split('&');

        foreach (string param in queryParams)
        {
            string[] keyValue = param.Split('=');
            if (keyValue.Length == 2)
            {
                string key = Uri.UnescapeDataString(keyValue[0]);
                string value = Uri.UnescapeDataString(keyValue[1]);
                queryDict[key] = value;
            }
        }

        return queryDict;
    }
}
