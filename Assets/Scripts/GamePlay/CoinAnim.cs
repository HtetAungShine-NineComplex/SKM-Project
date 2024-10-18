using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinAnim : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;
    public float speed;

    private Vector2 _startPos;
    private Transform _targetPos;

    private Vector2 _randomTargetPos;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 parentSize = GetComponentInParent<CoinAnimController>().transform.localScale;

        transform.localScale = new Vector3(0.65f / parentSize.x, 0.65f / parentSize.y, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {

        

        

        float currentDistance = Vector2.Distance(rectTransform.position, _randomTargetPos);
        if (currentDistance <= 0.1f)
        {
            return;
        }

        // Move towards the random target position
        rectTransform.position = Vector2.Lerp(rectTransform.position, _randomTargetPos, Time.deltaTime * speed);
    }


    public void SetPositions(Vector2 start, Transform targetPOs)
    {
        _startPos = start;
        _targetPos = targetPOs;

        // Define a range for the random offset
        float randomRange = 30f; // Adjust this range as needed

        // Generate a random offset around the target position
        Vector2 randomOffset = new Vector2(
            Random.Range(-randomRange, randomRange),
            Random.Range(-randomRange, randomRange)
        );

        // Calculate a random target position based on the original _targetPos and randomOffset
        _randomTargetPos = (Vector2)_targetPos.position + randomOffset;

        // Set the starting position
        rectTransform.position = start;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
