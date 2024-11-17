using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetPanelAmountController : MonoBehaviour
{
    [SerializeField] private TMP_Text _bet1Btn;
    [SerializeField] private TMP_Text _bet2Btn;
    [SerializeField] private TMP_Text _bet3Btn;
    [SerializeField] private TMP_Text _maxBtn;
    [SerializeField] private TMP_Text _sliderValueTxt;
    [SerializeField] private Slider _betSlider;
    [SerializeField] private GameObject _sliderPanel;
    [SerializeField] private GameObject _sliderBtn;

    public int SliderBetValue => (int)_betSlider.value;

    private void LateUpdate()
    {
        _sliderValueTxt.text = "" + (int)_betSlider.value;
    }

    public void SetBtnAmounts(int bet1, int bet2, int bet3, int maxBet)
    {
        _bet1Btn.text = bet1.ToString();
        _bet2Btn.text = bet2.ToString();
        _bet3Btn.text = bet3.ToString();
        _maxBtn.text = "MAX : " + maxBet.ToString();

        _betSlider.minValue = bet1;
        _betSlider.maxValue = maxBet;
        _betSlider.value = _betSlider.minValue;

        _sliderBtn.SetActive(true);
        _sliderPanel.SetActive(false);
    }
}
