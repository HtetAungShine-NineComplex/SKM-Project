using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardViewPanel : MonoBehaviour
{
    public static CardViewPanel Instance;

    [SerializeField] private List<CardDemo> _cards = new List<CardDemo>();
    [SerializeField] private GameObject _root;
    [SerializeField] private GameObject _actionBtnsRoot;
    [SerializeField] private GameObject _doneBtn;
    [SerializeField] private GameObject _anim;
    [SerializeField] private float _distanceOffset;
    [SerializeField] private DraggableCard _draggableCard;
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _catchBtn;

    private float _duration = 30f; // Duration in seconds
    private float _elapsedTime = 0f;

    private RoomUserItem _myItem;

    private bool _isDo;
    private int _total;
    private bool _hasThirdCard;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _slider.value = 1;
    }

    private void Update()
    {
        if(_elapsedTime >= _duration && _root.activeSelf)
        {
            ClosePanel();
        }

        // Calculate the positional difference between the two cards
        Vector3 difference = _cards[1].transform.position - _cards[0].transform.position;

        // Check if the x (right) or y (bottom) components exceed the offset
        if (difference.x >= _distanceOffset || difference.y <= -_distanceOffset)
        {
            //_cards[1].transform.position = _cards[0].transform.position;

            if (_isDo)
            {
                ToggleActionPanel(false);
            }
            else
            {
                if (!_hasThirdCard)
                {
                    ToggleActionPanel(true);
                }
            }
            // ClosePanel(); Uncomment if you want to close the panel here
        }
    }

    private void LateUpdate()
    {
        if (_slider != null && _elapsedTime < _duration && _root.activeSelf)
        {
            _elapsedTime += Time.deltaTime;
            // Lerp slider value from 1 to 0 over the specified duration
            _slider.value = Mathf.Lerp(1f, 0f, _elapsedTime / _duration);
        }
    }

    public void SetTwoCardsAndShow(string firstCard, string secCard, RoomUserItem myItem, bool isDo = false, int total = 0)
    {
        if (_root.activeSelf)
        {
            return;
        }
        _myItem = myItem;
        _isDo = isDo;
        _total = total;

        _cards[0].SetCard(firstCard);
        _cards[1].SetCard(secCard);
        _elapsedTime = 0;
        _slider.value = 1;
        _root.SetActive(true);
        _hasThirdCard = false;

        _actionBtnsRoot.SetActive(false);
        _doneBtn.SetActive(false);
        _anim.SetActive(false);
        
    }

    public void OnDrawCard(string drawnCard)
    {
        _cards[0].SetCard(drawnCard);
        _hasThirdCard = true;
        ToggleActionPanel(false);
        ToggleCatchBtn(false);
        _anim.SetActive(false);
        _anim.SetActive(true);
        _myItem.EndTurn();
    }

    public void ToggleActionPanel(bool isOn)
    {
        if (_myItem.IsBank && !_catchBtn.gameObject.activeSelf && !_actionBtnsRoot.activeSelf)
        {
            ToggleCatchBtn(true);
        }

        _actionBtnsRoot.SetActive(isOn);
        _doneBtn.SetActive(!isOn);

        
    }

    public void ToggleCatchBtn(bool isOn)
    {
        if (isOn)
        {
            StartCoroutine(CatchButtonDelay());
        }
        else
        {
            _catchBtn.gameObject.SetActive(isOn);
        }
    }

    IEnumerator CatchButtonDelay()
    {
        _catchBtn.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);

        _catchBtn.gameObject.SetActive(false);
    }

    public void ClosePanel()
    {
        if (!_root.activeSelf)
        {
            return;
        }

        StopAllCoroutines();

        if (_isDo)
        {
            _myItem.PlayerDo(_total);

            if (_total == 8)
            {
                Managers.AudioManager.Play8DoClip();
            }
            else if (_total == 9)
            {
                Managers.AudioManager.Play9DoClip();
            }
        }
        _draggableCard.isDragging = false;
        _cards[1].transform.position = _cards[0].transform.position;
        _root.SetActive(false);
        ToggleCatchBtn(false);
    }
}

