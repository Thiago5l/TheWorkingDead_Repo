using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePieceUI : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [HideInInspector] public UIPuzzleManager manager;
    [HideInInspector] public int pieceIndex;

    public bool isCorrect;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private bool dragging;
    private Vector2 offset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent as RectTransform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isCorrect) return;

        dragging = true;
        rectTransform.SetAsLastSibling();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
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
            parentRect,
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
