using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardViewPanel : MonoBehaviour
{
    public static CardViewPanel Instance;

    [SerializeField] private List<CardDemo> _cards = new List<CardDemo>();
    [SerializeField] private GameObject _root;
    [SerializeField] private float _distanceOffset;
    [SerializeField] private DraggableCard _draggableCard;
    [SerializeField] private Slider _slider;

    private float _duration = 7f; // Duration in seconds
    private float _elapsedTime = 0f;

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
        if(_elapsedTime >= 3 && _root.activeSelf)
        {
            ClosePanel();
        }

        float dis = Vector3.Distance(_cards[1].transform.position, _cards[0].transform.position);

        if (dis >= _distanceOffset)
        {
            _cards[1].transform.position = _cards[0].transform.position;
            ClosePanel();
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

    public void SetTwoCardsAndShow(string firstCard, string secCard)
    {
        if (_root.activeSelf)
        {
            return;
        }

        _cards[0].SetCard(firstCard);
        _cards[1].SetCard(secCard);
        _elapsedTime = 0;
        _slider.value = 1;
        _root.SetActive(true);
    }

    public void ClosePanel()
    {
        if (!_root.activeSelf)
        {
            return;
        }

        _draggableCard.isDragging = false;
        _cards[1].transform.position = _cards[0].transform.position;
        _root.SetActive(false);
    }
}

