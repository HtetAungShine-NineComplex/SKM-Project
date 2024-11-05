using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinAnim : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;
    public TMP_Text txt;
    public float speed;
    public int value;

    private Vector2 _startPos;
    private Vector3 _targetPos;

    private Vector2 _randomTargetPos;

    private bool _destroyAtTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<CoinAnimController>() == null)
        {
            return;
        }

        Vector3 parentSize = GetComponentInParent<CoinAnimController>().transform.localScale;

        transform.localScale = new Vector3(0.65f / parentSize.x, 0.65f / parentSize.y, 2);
    }

    void Update()
    {

        float currentDistance = Vector2.Distance(rectTransform.position, _randomTargetPos);
        if (currentDistance <= 0.01f)
        {
            if (_destroyAtTarget)
            {
                Destroy(gameObject);
            }

            return;
        }

        // Move towards the random target position
        rectTransform.position = Vector2.Lerp(rectTransform.position, _randomTargetPos, Time.deltaTime * speed);
    }


    public void SetPositions(Vector2 start, Vector3 targetPOs, bool destroy = false, float sped = 6f)
    {
        _startPos = start;
        _targetPos = targetPOs;
        _destroyAtTarget = destroy;
        speed = sped;

        // Define a range for the random offset
        //float randomRange = 30f; // Adjust this range as needed

        // Generate a random offset around the target position
       /* Vector2 randomOffset = new Vector2(
            Random.Range(-randomRange, randomRange),
            Random.Range(-randomRange, randomRange)
        );*/

        // Calculate a random target position based on the original _targetPos and randomOffset
        _randomTargetPos = (Vector2)_targetPos;

        // Set the starting position
        rectTransform.position = start;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetValueString(string text)
    {
        txt.text = text;
    }
}
