using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class Level4Puzzle : MonoBehaviour
{
    [Header("Correct Piece Highlights")]
    public GameObject highlight_2_1;
    public GameObject highlight_2_2;
    public GameObject highlight_3_1;
    public GameObject highlight_3_2;

    [Header("Popups")]
    public GameObject popupError;
    public GameObject popupSuccess;
    public GameObject popupAdvice;

    [Header("Advice Text")]
    public TMP_Text adviceText;

    private bool selected_2_1 = false;
    private bool selected_2_2 = false;
    private bool selected_3_1 = false;
    private bool selected_3_2 = false;

    private bool levelCompleted = false;
    
    public UnityEvent OnAdvice;
    public UnityEvent OnSuccess;

    public void ClickCorrect_2_1()
    {
        if (levelCompleted) return;

        selected_2_1 = true;

        if (highlight_2_1 != null)
            highlight_2_1.SetActive(true);

        CheckCompletion();
    }

    public void ClickCorrect_2_2()
    {
        if (levelCompleted) return;

        selected_2_2 = true;

        if (highlight_2_2 != null)
            highlight_2_2.SetActive(true);

        CheckCompletion();
    }

    public void ClickCorrect_3_1()
    {
        if (levelCompleted) return;

        selected_3_1 = true;

        if (highlight_3_1 != null)
            highlight_3_1.SetActive(true);

        CheckCompletion();
    }

    public void ClickCorrect_3_2()
    {
        if (levelCompleted) return;

        selected_3_2 = true;

        if (highlight_3_2 != null)
            highlight_3_2.SetActive(true);

        CheckCompletion();
    }

    public void ClickWrongPiece()
    {
        if (levelCompleted) return;

        if (popupError != null)
            popupError.SetActive(true);
    }

    private void CheckCompletion()
    {
        if (selected_2_1 && selected_2_2 && selected_3_1 && selected_3_2)
        {
            levelCompleted = true;

            if (popupSuccess != null)
                popupSuccess.SetActive(true);
        }
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
            adviceText.text = "Гүтгэсэн худал үгс, эвлүүлсэн зураг зэргийг баримт болгон хадгалж байгаарай.";

        if (popupAdvice != null)
            popupAdvice.SetActive(true);
        OnAdvice.Invoke();
    }

    public void CloseAdvice()
    {
        if (popupAdvice != null)
            popupAdvice.SetActive(false);
        
        OnSuccess.Invoke();
        Debug.Log("Level 4 complete");
    }

    public void ResetLevel()
    {
        selected_2_1 = false;
        selected_2_2 = false;
        selected_3_1 = false;
        selected_3_2 = false;
        levelCompleted = false;

        if (highlight_2_1 != null) highlight_2_1.SetActive(false);
        if (highlight_2_2 != null) highlight_2_2.SetActive(false);
        if (highlight_3_1 != null) highlight_3_1.SetActive(false);
        if (highlight_3_2 != null) highlight_3_2.SetActive(false);
    }
}
