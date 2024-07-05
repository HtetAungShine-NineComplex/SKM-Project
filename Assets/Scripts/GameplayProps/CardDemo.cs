using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDemo : MonoBehaviour
{
    [SerializeField] private Image _mainImg;
    [SerializeField] private Sprite _backSprite;

    public bool IsBlank {  get; private set; }

    private void Start()
    {
    }

    public void SetCard(string cardName)
    {
        _mainImg.sprite = Managers.CardDataManager.GetCardSprite(cardName);
        IsBlank = false;
    }

    public void ResetCard()
    {
        _mainImg.sprite = _backSprite;
        IsBlank = true;
    }
}
