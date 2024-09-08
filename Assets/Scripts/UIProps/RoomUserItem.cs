using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class RoomUserItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameTxt;
    [SerializeField] private TMP_Text _totalAmountTxt;
    [SerializeField] private TMP_Text _totalValueTxt;
    [SerializeField] private TMP_Text _betAmountTxt;
    [SerializeField] private GameObject _bankerStatus;

    [SerializeField] private GameObject _8doObj;
    [SerializeField] private GameObject _9doObj;
    [SerializeField] private GameObject _winObj;
    [SerializeField] private GameObject _loseObj;
    [SerializeField] private GameObject _resultParentObj;

    [SerializeField] private Transform _playerCardsRoot;
    [SerializeField] private CardDemo _cardPrefab;
    [SerializeField] private List<CardDemo> _playerCurrentCards;

    [SerializeField] private float _cardSpacing;
    [SerializeField] private float _cardOffsetY;
    [SerializeField] private float _maxAngle = 30f;

    public int ID { get; private set; } // user id to access the user item
    public string Name { get; private set; } // user id to access the user item
    public int TotalAmount { get; private set; } // user id to access the user item

    private void Start()
    {
        _playerCurrentCards = new List<CardDemo>();
    }

    private void Update()
    {
        float half = (_playerCurrentCards.Count - 1) / 2f;

        for (int i = 0; i < _playerCurrentCards.Count; i++)
        {
            CardDemo card = _playerCurrentCards[i];

            // Calculate the horizontal position
            float xPos = (i - half) * _cardSpacing;

            // Calculate the vertical position for the fan effect
            float distanceFromCenter = Mathf.Abs(i - half);
            float yPos = -distanceFromCenter * distanceFromCenter * _cardOffsetY;

            // Calculate the rotation angle
            float angle = (i - half) / half * _maxAngle;

            // Set the target position
            card.targetPosition = new Vector2(xPos, yPos);

            // Set the rotation (assuming card has a method or property to set rotation)
            if(_playerCurrentCards.Count > 1)
            {
                card.targetRotation = Quaternion.Euler(0, 0, -angle);
            }
            else
            {
                card.targetRotation = Quaternion.identity;
            }
        }
    }

    //card.targetPosition = Vector2.zero;
    //card.deck_angle = (index - count_half) * -card_angle;
    public void SetId(int id)
    {
        ID = id;
    }
    public void SetName(string name)
    {
        Name = name;
        _nameTxt.text = name;
    }

    public void SetAmount(int amount)
    {
        TotalAmount = amount;
        _totalAmountTxt.text = amount.ToString();
    }

    public void SetTotalValue(int value)
    {
        _totalValueTxt.text = value.ToString();
    }

    public void SetBetAmount(int value)
    {
        if (value == 0)
        {
            _betAmountTxt.text = "";
        }
        else
        {
            _betAmountTxt.text = value.ToString();
        }
        
    }

    public void IsBanker()
    {
        _bankerStatus.SetActive(true);
    }

    public void IsPlayer()
    {
        _bankerStatus.SetActive(false);
    }

    public void WinLose(bool isWin)
    {
        StartCoroutine(ShowWinOrLose(isWin));
    }

    public void PlayerDo(int value) //8 or 9
    {
        if(value == 8)
        {
            _8doObj.SetActive(true);
        }
        else if (value == 9)
        {
            _9doObj.SetActive(true);
        }
    }

    [ContextMenu("Test")]
    public void AddBlankCards()
    {
        CardDemo card = Instantiate(_cardPrefab, _playerCardsRoot);
        card.ResetCard();
        _playerCurrentCards.Add(card);
    }

    public void AddCard(string cardName)
    {
        if(_playerCurrentCards.Count == 0)
        {
            CardDemo addedCard = Instantiate(_cardPrefab, _playerCardsRoot);
            addedCard.SetCard(cardName);
            _playerCurrentCards.Add(addedCard);

            return;
        }
        else
        {
            foreach (CardDemo card in _playerCurrentCards)
            {
                if (card.IsBlank)
                {
                    card.SetCard(cardName);
                    return;
                }
            }

            CardDemo addedCard = Instantiate(_cardPrefab, _playerCardsRoot);
            addedCard.SetCard(cardName);
            _playerCurrentCards.Add(addedCard);
            return;
        }
    }

    public void UpdateAllCards(string[] cards)
    {
        if (_playerCurrentCards.Count > 0)
        {
            foreach(CardDemo card in _playerCurrentCards)
            {
                Destroy(card.gameObject);
            }

            _playerCurrentCards = new List<CardDemo>();
        }

        foreach (string cardName in cards)
        {
            CardDemo addedCard = Instantiate(_cardPrefab, _playerCardsRoot);
            addedCard.SetCard(cardName);
            _playerCurrentCards.Add(addedCard);
        }
    }

    public void Reset()
    {
        SetTotalValue(0);
        SetBetAmount(0);
        _8doObj.SetActive(false);
        _9doObj.SetActive(false);

        foreach (CardDemo card in _playerCurrentCards)
        {
            Destroy(card.gameObject);
        }

        _playerCurrentCards.Clear();
    }
     
    IEnumerator ShowWinOrLose(bool isWin)
    {
        if(isWin)
        {
            _winObj.SetActive(true);
        }
        else
        {
            _loseObj.SetActive(true);
        }

        _resultParentObj.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        _loseObj.SetActive(false);
        _winObj.SetActive(false);
        
    }
}
