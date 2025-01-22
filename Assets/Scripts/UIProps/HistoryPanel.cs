using Newtonsoft.Json;
using Proyecto26;
using Shan.API;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryPanel : MonoBehaviour
{
    [SerializeField] private HistoryListBtn _listPrefab;
    [SerializeField] private Transform _root;
    [Space]
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private TMP_Text _gameIDTxt;
    [SerializeField] private TMP_Text _betTxt;
    [SerializeField] private TMP_Text _winLossAmountTxt;
    [SerializeField] private Image _winLossImg;
    [SerializeField] private Sprite _win;
    [SerializeField] private Sprite _loss;

    private List<HistoryListBtn> _list;

    private void OnEnable()
    {
        UpdateData();
    }

    public void UpdateData()
    {
        _gameIDTxt.text = string.Empty;
        _betTxt.text = string.Empty;
        _winLossAmountTxt.text = string.Empty;

        if(_list != null && _list.Count > 0)
        {
            foreach (HistoryListBtn list in _list)
            {
                Destroy(list.gameObject);
            }
        }

        _loadingPanel.SetActive(true);

        _list = new List<HistoryListBtn>();

        string uri = Managers.DataLoader.NetworkData.URI + Managers.DataLoader.NetworkData.gameHistories;

#if UNITY_WEBGL && UNITY_EDITOR
        PlayerPrefs.SetString("token", Managers.DataLoader.NetworkData.testToken);
#endif

        RequestHelper currentRequest = new RequestHelper
        {
            Uri = uri,
            Headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + PlayerPrefs.GetString("token") }
            }
          ,
            EnableDebug = true
        };
        RestClient.Get(currentRequest)
        .Then(res => {
            GameHistoryResponse r = JsonConvert.DeserializeObject<GameHistoryResponse>(res.Text);
            if (r.status == "success")
            {
                foreach(GameHistoryData history in r.data)
                {
                    HistoryListBtn btn = Instantiate(_listPrefab, _root);
                    btn.SetPanel(this);
                    btn.SetData(history);
                    _list.Add(btn);
                }

                _list[0].OnButtonClick();
                _loadingPanel.SetActive(false);
            }
        })
        .Catch(err => Debug.Log(err.Message));

        
        
    }

    public void UpdateSidePanel(string gameID, int bet, int amountChanged, string winLoss)
    {
        _gameIDTxt.text = gameID;
        _betTxt.text = bet.ToString();
        _winLossAmountTxt.text = amountChanged.ToString();

        if (winLoss == "win")
        {
            _winLossImg.sprite = _win;
        }
        else
        {
            _winLossImg.sprite = _loss;
        }
    }
}
