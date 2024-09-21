using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CardAnim : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    public float speed;
    public float rotationSpeed = 5f;  // Speed of rotation
    public float fadeSpeed = 2f;      // Speed at which canvas fades

    private Vector2 _startPos;
    private PlayerPos _targetPos;
    private float _totalDistance;

    public string addCardName;
    public bool hasCard = false;
    public bool isDraw = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize canvasGroup alpha to fully visible
        canvasGroup.alpha = 1f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Move towards the target position
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, _targetPos.rectTransform.anchoredPosition, Time.deltaTime * speed);

        // Calculate the direction vector
        Vector2 direction = _targetPos.rectTransform.anchoredPosition - rectTransform.anchoredPosition;

        // Calculate angle between the current position and the target position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust the angle to point the Y-axis toward the target
        float adjustedAngle = angle - 90f;

        // Smoothly rotate towards the target position
        rectTransform.rotation = Quaternion.Lerp(rectTransform.rotation, Quaternion.Euler(0, 0, adjustedAngle), Time.deltaTime * rotationSpeed);

        // Calculate the remaining distance to the target
        float currentDistance = Vector2.Distance(rectTransform.anchoredPosition, _targetPos.rectTransform.anchoredPosition);

        // Start fading when the card has traveled half of the total distance
        if (currentDistance < _totalDistance / 2)
        {
            // Gradually fade the canvasGroup alpha
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, Time.deltaTime * fadeSpeed);
        }

        if (currentDistance <= 30f)
        {
            if (hasCard)
            {
                _targetPos.currentUser?.AddCard(addCardName, isDraw);
            }
            else
            {
                _targetPos.currentUser?.AddBlankCards();
            }
            Destroy(this.gameObject);
        }
    }

    public void SetPositions(Vector2 start, PlayerPos targetPOs)
    {
        _startPos = start;
        _targetPos = targetPOs;

        // Set the starting position
        rectTransform.anchoredPosition = start;

        // Calculate the total distance between start and target
        _totalDistance = Vector2.Distance(start, targetPOs.rectTransform.anchoredPosition);

        // Ensure the canvasGroup starts fully visible
        canvasGroup.alpha = 1f;
    }
}
