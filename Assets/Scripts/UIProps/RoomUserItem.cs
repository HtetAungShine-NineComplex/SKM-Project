using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomUserItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameTxt;
    [SerializeField] private TMP_Text _totalValueTxt;
    [SerializeField] private GameObject _bankerStatus;
    [SerializeField] private TMP_Text _winLoseTxt;
    [SerializeField] private GameObject _8doObj;
    [SerializeField] private GameObject _9doObj;

    [SerializeField] private Transform _playerCardsRoot;
    [SerializeField] private CardDemo _cardPrefab;
    [SerializeField] private List<CardDemo> _playerCurrentCards;

    public int ID { get; private set; } // user id to access the user item
    public string Name { get; private set; } // user id to access the user item

    private void Start()
    {
        _playerCurrentCards = new List<CardDemo>();
    }

    public void SetId(int id)
    {
        ID = id;
    }

    public void SetName(string name)
    {
        Name = name;
        _nameTxt.text = name;
    }

    public void SetTotalValue(int value)
    {
        _totalValueTxt.text = value.ToString();
    }

    public void IsBanker()
    {
        _bankerStatus.SetActive(true);
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
            _winLoseTxt.text = "WIN";
        }
        else
        {
            _winLoseTxt.text = "LOSE";
        }

        _winLoseTxt.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        _winLoseTxt.gameObject.SetActive(false);
    }
}
