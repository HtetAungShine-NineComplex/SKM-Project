using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform _panelSafeArea;
    private Rect _currentSafeArea = new Rect(0, 0, 0, 0);
    private ScreenOrientation _currentOrientation = ScreenOrientation.AutoRotation;

    void Start()
    {
        _panelSafeArea = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        if (_currentSafeArea != Screen.safeArea || _currentOrientation != Screen.orientation)
        {
            ApplySafeArea();
        }
    }

    void ApplySafeArea()
    {
        _currentSafeArea = Screen.safeArea;
        _currentOrientation = Screen.orientation;

        // Convert safe area rectangle from screen space to anchor min and max
        var anchorMin = _currentSafeArea.position;
        var anchorMax = _currentSafeArea.position + _currentSafeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _panelSafeArea.anchorMin = anchorMin;
        _panelSafeArea.anchorMax = anchorMax;

        Debug.Log($"Safe Area applied: {anchorMin} to {anchorMax}");
    }
}
