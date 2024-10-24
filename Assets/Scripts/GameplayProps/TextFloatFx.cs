using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFloatFx : MonoBehaviour
{
    [SerializeField] private TMP_Text _amountTxt;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _transform;

    private bool _move = false;

    private void Start()
    {
        _move = true;
        _speed *= Screen.height / 800;

        Destroy(gameObject, 6f);
    }


    public void SetParent(Transform root)
    {
        if (_transform.parent != root)
        {
            _transform.SetParent(root);
        }
    }

    private void Update()
    {
        if (!_move)
        {
            return;
        }

        // Move the text upwards
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // Fade out the text over time
        float alpha = _canvasGroup.alpha;
        alpha -= Time.deltaTime * 0.2f;
        _canvasGroup.alpha = alpha;
    }

    public void SetAmount(string init, int amount)
    {
        _amountTxt.text = init + amount.ToString();
    }
}
