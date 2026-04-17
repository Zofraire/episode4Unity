using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Level5SliderPuzzle : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;
    public RectTransform puzzlePiece;
    public RectTransform slotTarget;

    [Header("Movement")]
    public float maxX = 120f;
    public float tolerance = 12f;

    [Header("Popups")]
    public GameObject popupError;
    public GameObject popupSuccess;
    public GameObject popupAdvice;

    [Header("Advice Text")]
    public TMP_Text adviceText;

    private bool levelCompleted = false;
    private float startX;
    public UnityEvent OnSuccess;
    public UnityEvent OnAdvice;

    private void Start()
    {
        if (puzzlePiece != null)
        {
            startX = puzzlePiece.anchoredPosition.x;
        }

        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.wholeNumbers = false;
            slider.onValueChanged.AddListener(OnSliderChanged);
        }

        ResetLevel();
    }

    public void OnSliderChanged(float value)
    {
        if (levelCompleted) return;
        if (puzzlePiece == null) return;

        float newX = Mathf.Lerp(startX, maxX, value);

        Vector2 pos = puzzlePiece.anchoredPosition;
        pos.x = newX;
        puzzlePiece.anchoredPosition = pos;

        CheckAutoSuccess();
    }

    private void CheckAutoSuccess()
    {
        if (levelCompleted) return;
        if (puzzlePiece == null || slotTarget == null) return;

        float currentX = puzzlePiece.anchoredPosition.x;
        float targetX = slotTarget.anchoredPosition.x;

        if (Mathf.Abs(currentX - targetX) <= tolerance)
        {
            levelCompleted = true;

            Vector2 pos = puzzlePiece.anchoredPosition;
            pos.x = targetX;
            puzzlePiece.anchoredPosition = pos;

            if (slider != null)
                slider.interactable = false;

            if (popupSuccess != null)
                popupSuccess.SetActive(true);
        }
    }

    public void ShowError()
    {
        if (popupError != null)
            popupError.SetActive(true);
    }

    public void CloseError()
    {
        if (popupError != null)
            popupError.SetActive(false);
    }

    public void CloseSuccess()
    {
        if (popupSuccess != null)
            popupSuccess.SetActive(false);

        if (adviceText != null)
            adviceText.text = "Хүний зургийг зөвшөөрөлгүй нийтэлж болохгүй шүү.";

        if (popupAdvice != null)
            popupAdvice.SetActive(true);
            
        OnAdvice.Invoke();
    }

    public void CloseAdvice()
    {
        if (popupAdvice != null)
            popupAdvice.SetActive(false);
        
        OnSuccess.Invoke();
        Debug.Log("Level 5 complete");
    }

    public void ResetLevel()
    {
        levelCompleted = false;

        if (slider != null)
        {
            slider.interactable = true;
            slider.value = 0f;
        }

        if (puzzlePiece != null)
        {
            Vector2 pos = puzzlePiece.anchoredPosition;
            pos.x = startX;
            puzzlePiece.anchoredPosition = pos;
        }
    }
}
