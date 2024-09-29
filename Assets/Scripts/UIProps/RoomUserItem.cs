using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class RoomUserItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameTxt;
    [SerializeField] private TMP_Text _totalAmountTxt;
    [SerializeField] private TMP_Text _totalValueTxt;
    [SerializeField] private TMP_Text _betAmountTxt;
    [SerializeField] private GameObject _bankerStatus;
    [SerializeField] private TextFloatFx _textFloatPrefab;

    [SerializeField] private GameObject _8doObj;
    [SerializeField] private GameObject _9doObj;
    [SerializeField] private GameObject _x2Obj;
    [SerializeField] private GameObject _x3Obj;
    [SerializeField] private GameObject[] _winObj;
    [SerializeField] private GameObject _loseObj;
    [SerializeField] private GameObject _resultParentObj;
    [SerializeField] private GameObject _loadingObject;
    [SerializeField] private Image _cooldownFillImage;

    [SerializeField] private RectTransform _playerCardsPanel;
    [SerializeField] private Transform _playerCardsRoot;
    [SerializeField] private CardDemo _cardPrefab;
    [SerializeField] private List<CardDemo> _playerCurrentCards;
    [SerializeField] private Color _loseColor;

    [SerializeField] private float _cardSpacing;
    [SerializeField] private float _cardOffsetY;
    [SerializeField] private float _maxAngle = 30f;

    public int ID { get; private set; } // user id to access the user item
    public string Name { get; private set; } // user id to access the user item
    public int TotalAmount { get; private set; } // user id to access the user item
    public int TotalCardValue { get; private set; } // user id to access the user item
    public bool IsBank { get; private set; }

    private bool _isMyTurn = false;
    private bool _isBetting = false;
    private float _duration = 15f; // Duration in seconds
    private float _elapsedTime = 0f;

    private Vector3 _originalCardPos;

    private void Awake()
    {
        _originalCardPos = _playerCardsPanel.anchoredPosition;
    }

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

    private void LateUpdate()
    {
        if ((_cooldownFillImage != null && _elapsedTime < _duration) && (_isMyTurn || _isBetting))
        {
            _elapsedTime += Time.deltaTime;
            // Lerp slider value from 1 to 0 over the specified duration
            _cooldownFillImage.fillAmount = Mathf.Lerp(1f, 0f, _elapsedTime / _duration);
        }
    }

    public void ToggleLoadingObject(bool toggle)
    {
        if (Name == GlobalManager.Instance.GetSfsClient().MySelf.Name)
        {
            _loadingObject.SetActive(false);
            return;
        }

        _loadingObject.SetActive(toggle);
    }

    public void SetBankObject(GameObject obj)
    {
        _bankerStatus = obj;
    }

    public void StartTurn()
    {
        _duration = 15;
        _elapsedTime = 0;
        _isMyTurn = true;
        _cooldownFillImage.fillAmount = 1;
        _cooldownFillImage.gameObject.SetActive(true);
    }


    public void EndTurn()
    {
        _isMyTurn = false;
        _cooldownFillImage.fillAmount = 0;
        _cooldownFillImage.gameObject.SetActive(false);
    }

    public void StartBet()
    {
        _duration = 7;
        _cooldownFillImage.fillAmount = 1;
        _elapsedTime = 0;
        _cooldownFillImage.gameObject.SetActive(true);
        _isBetting = true;
    }

    public void EndBet()
    {
        _cooldownFillImage.fillAmount = 0;
        _cooldownFillImage.gameObject.SetActive(false);
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

    public void SetModifier(int amount)
    {
        _x2Obj.SetActive(false);
        _x3Obj.SetActive(false);

        Debug.Log("Modifier : " + amount);

        if (amount == 1)
        {

            return;
        }
        else if(amount == 2)
        {
            _x2Obj.SetActive(true);
        }    
        else if(amount == 3)
        {
            _x3Obj.SetActive(true);
        }
    }

    public void SetTotalValue(int value)
    {
        _totalValueTxt.text = value.ToString();
        TotalCardValue = value;
    }

    public void ShowCards()
    {
        _playerCardsPanel.transform.localPosition = Vector3.zero;

        if (IsBank)
        {
            StartCoroutine(ShowingBankCards());
        }
        else
        {
            _playerCardsPanel.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
        }
    }

    IEnumerator ShowingBankCards()
    {
        _playerCardsPanel.transform.localScale = new Vector3(2.3f, 2.3f, 1f);

        yield return new WaitForSeconds(3f);

        _playerCardsPanel.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
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
        IsBank = true;
    }

    public void IsPlayer()
    {
        _bankerStatus.SetActive(false);
        IsBank = false;
    }

    public void WinLose(bool isWin, int amountChanged)
    {
        StartCoroutine(ShowWinOrLose(isWin, amountChanged));
    }

    public void PlayerDo(int value) //8 or 9
    {
        ToggleLoadingObject(false);

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

    public string[] GetCardsArray()
    {
        string[] cards = new string[_playerCurrentCards.Count];
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = _playerCurrentCards[i].Name;
        }
        return cards;
    }
    public void Reset()
    {
        ToggleLoadingObject(false);

        _playerCardsPanel.anchoredPosition = _originalCardPos;
        _playerCardsPanel.localScale = new Vector3(1.4f, 1.4f, 1f);

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
     
    IEnumerator ShowWinOrLose(bool isWin, int amountChanged)
    {
        if(isWin)
        {
            TextFloatFx fx = Instantiate(_textFloatPrefab, transform);
            fx.SetAmount("+", amountChanged);
            foreach (GameObject item in _winObj)
            {
                item.SetActive(true);
            }
        }
        else
        {
            TextFloatFx fx = Instantiate(_textFloatPrefab, transform);
            fx.SetAmount("-", amountChanged);
            foreach (CardDemo card in _playerCurrentCards)
            {
                card.SetColor(_loseColor);
            }
            _loseObj.SetActive(true);
        }

        _resultParentObj.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        _loseObj.SetActive(false);
        foreach (CardDemo card in _playerCurrentCards)
        {
            card.SetColor(Color.white);
        }
        foreach (GameObject item in _winObj)
        {
            item.SetActive(false);
        }
        
    }
}
