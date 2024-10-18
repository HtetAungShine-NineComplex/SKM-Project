using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BetPanelAmountController : MonoBehaviour
{
    [SerializeField] private TMP_Text _bet1Btn;
    [SerializeField] private TMP_Text _bet2Btn;
    [SerializeField] private TMP_Text _bet3Btn;
    [SerializeField] private TMP_Text _maxBtn;

    public void SetBtnAmounts(int bet1, int bet2, int bet3, int maxBet)
    {
        _bet1Btn.text = bet1.ToString();
        _bet2Btn.text = bet2.ToString();
        _bet3Btn.text = bet3.ToString();
        _maxBtn.text = "MAX : " + maxBet.ToString();
    }
}
