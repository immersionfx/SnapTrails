using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 startPos;
    private Vector2 endPos;
    public RectTransform rectTransform;
    public float minY;
    public float maxY;

    private bool isDragging = false;

    public float lerpSpeed = 5f; // Adjust the speed of the Lerp animation here

    private Vector2 targetPosition;

    private void Start()
    {
        // Set the initial position to minY
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, minY);
        targetPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        startPos = eventData.position;
        targetPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        endPos = eventData.position;

        // Determine the target position based on whether it should be minY or maxY
        targetPosition.y = (targetPosition.y - minY) < (maxY - targetPosition.y) ? maxY : minY;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 offset = eventData.position - startPos;
            Vector2 newPosition = rectTransform.anchoredPosition + offset;
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            newPosition.x = 0;
            rectTransform.anchoredPosition = newPosition;

            startPos = eventData.position;
        }
    }

    void Update()
    {
        if (!isDragging)
        {
            // Smoothly move the window to the target position using Lerp
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, lerpSpeed * Time.deltaTime);
        }
    }
}

