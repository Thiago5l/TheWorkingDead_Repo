using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePieceUI : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    public UIPuzzleManager manager;
    public int pieceIndex;
    public bool isCorrect;

    private RectTransform rectTransform;
    private Canvas canvas;
    private bool dragging;
    private Vector2 offset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = manager.canvas;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isCorrect) return;

        dragging = true;
        rectTransform.SetAsLastSibling();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            manager.canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mousePos
        );

        offset = rectTransform.anchoredPosition - mousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || isCorrect) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            manager.canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 mousePos
        );

        rectTransform.anchoredPosition = mousePos + offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!dragging) return;

        dragging = false;
        manager.TrySnap(this);
    }
}
