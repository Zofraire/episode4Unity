using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePieceUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform correctSlot;
    public float snapDistance = 50f;

    [HideInInspector] public bool isPlaced = false;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 startPosition;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        canvasGroup.blocksRaycasts = true;

        float distance = Vector2.Distance(rectTransform.anchoredPosition, correctSlot.anchoredPosition);

        if (distance <= snapDistance)
        {
            rectTransform.anchoredPosition = correctSlot.anchoredPosition;
            isPlaced = true;

            Level5PuzzleManager manager = FindObjectOfType<Level5PuzzleManager>();
            if (manager != null)
                manager.CheckCompletion();
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
    }
}