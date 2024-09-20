using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isActive;

    private Vector3 _offset;
    private Vector3 _initialPos;

    public bool isDragging = false;

    private void Start()
    {
        _initialPos = transform.position;
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(isDragging)
        {
            transform.position = Input.mousePosition + _offset;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _initialPos, 4 * Time.deltaTime);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - Input.mousePosition;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isDragging)
        {
            return;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}
