using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDemo : MonoBehaviour
{
    [SerializeField] private Image _mainImg;
    [SerializeField] private Sprite _backSprite;
    [SerializeField] private RectTransform _transform;
    [SerializeField] private float _speed;

    public Vector2 targetPosition;
    public Quaternion targetRotation;

    public bool IsBlank {  get; private set; }

    private void Start()
    {
        _transform.anchoredPosition = Vector2.zero;
        targetRotation = Quaternion.identity;
    }

    private void LateUpdate()
    {
        _transform.anchoredPosition = Vector2.Lerp(_transform.anchoredPosition, targetPosition, Time.deltaTime * _speed);

        // Smoothly rotate the card to the target rotation
        _transform.localRotation = Quaternion.Lerp(_transform.localRotation, targetRotation, Time.deltaTime * _speed);
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
