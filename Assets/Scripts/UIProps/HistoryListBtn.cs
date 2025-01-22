using Shan.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryListBtn : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _winSprite;
    [SerializeField] private Sprite _loseSprite;

    private GameHistoryData _historyData;
    private Participant _curParticipant;

    private HistoryPanel _panel;

    public void SetData(GameHistoryData data)
    {
        _historyData = data;

        foreach (Participant p in _historyData.game_info.participants)
        {
            if(p.username == Managers.DataLoader.CurrentName)
            {
                _curParticipant = p;

                if(p.result.win_loss == "win")
                {
                    _img.sprite = _winSprite;
                }
                else
                {
                    _img.sprite = _loseSprite;
                }
            }
        }
    }

    public GameHistoryData GetHistoryData()
    {
        return _historyData;
    }

    public Participant GetParticipant() 
    { 
        return _curParticipant;
    }

    public void SetPanel(HistoryPanel panel)
    {
        _panel = panel;
    }

    public void OnButtonClick()
    {
        _panel.UpdateSidePanel(_historyData.match_id, _curParticipant.bet_info.bet_amount, _curParticipant.result.amount_won_lost, _curParticipant.result.win_loss);
    }
}
