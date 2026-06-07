using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Level5PuzzleManager : MonoBehaviour
{
    public PuzzlePieceUI[] pieces;

    public GameObject popupError;
    public GameObject popupSuccess;
    public GameObject popupAdvice;

    public TMP_Text adviceText;

    private bool levelCompleted = false;

    public UnityEvent OnSuccess;
    public UnityEvent OnAdvice;

    public void CheckCompletion()
    {
        if (levelCompleted) return;

        foreach (PuzzlePieceUI piece in pieces)
        {
            if (!piece.isPlaced)
                return;
        }

        levelCompleted = true;

        if (popupSuccess != null)
            popupSuccess.SetActive(true);
    }

    public void CloseError()
    {
        if (popupError != null)
            popupError.SetActive(false);
    }

    public void CloseSuccess()
    {
        

        if (popupSuccess != null)
            popupSuccess.SetActive(false);;

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
}
