using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public float xOffSet;
    public float yOffSet;
    Vector2 pointerOffset;
    RectTransform panelRectTransform;
    RectTransform canvasRectTransform;
    Canvas canvas;

    void Start()
    {

    }

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
            panelRectTransform = GetComponent<RectTransform>();
            xOffSet = ((panelRectTransform.rect.width * canvas.scaleFactor) / 2);
            yOffSet = ((panelRectTransform.rect.height * canvas.scaleFactor) / 2);
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        panelRectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out pointerOffset);
    }

    public void OnDrag(PointerEventData data)
    {
        if (panelRectTransform == null)
            return;

        Vector2 pointerPostion = ClampToWindow(data);

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, pointerPostion, data.pressEventCamera, out localPointerPosition
        ))
        {
           // print("Before: " + panelRectTransform.localPosition);
            panelRectTransform.anchoredPosition = (localPointerPosition - pointerOffset);
           // print("After 1: " + panelRectTransform.localPosition);

        }
       // print("After 2: " + panelRectTransform.localPosition);
    }
    Vector2 ClampToWindow(PointerEventData data)
    {
        Vector2 rawPointerPosition = data.position;

        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);

        float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x + xOffSet, canvasCorners[2].x - xOffSet);
        float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y + yOffSet, canvasCorners[2].y - yOffSet);

        Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
        return newPointerPosition;
    }
}