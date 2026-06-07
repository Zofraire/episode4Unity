using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Level6ShapeOrder : MonoBehaviour
{
    [Header("Highlights")]
    public GameObject circleHighlight;
    public GameObject squareHighlight;
    public GameObject triangleHighlight;

    [Header("Popups")]
    public GameObject popupError;
    public GameObject popupSuccess;
    public GameObject popupAdvice;

    [Header("Advice Text")]
    public TMP_Text adviceText;

    private int currentStep = 0;
    private bool levelCompleted = false;
    
    public UnityEvent OnAdvice;
    public UnityEvent OnSuccess;

    private void Start()
    {
        ResetLevel();
    }

    public void ClickCircle()
    {
        if (levelCompleted) return;
        CheckShape("Circle");
    }

    public void ClickSquare()
    {
        if (levelCompleted) return;
        CheckShape("Square");
    }

    public void ClickTriangle()
    {
        if (levelCompleted) return;
        CheckShape("Triangle");
    }

    private void CheckShape(string shapeName)
    {
        string[] correctOrder = { "Circle", "Square", "Triangle" };

        if (shapeName == correctOrder[currentStep])
        {
            if (shapeName == "Circle" && circleHighlight != null)
                circleHighlight.SetActive(true);

            if (shapeName == "Square" && squareHighlight != null)
                squareHighlight.SetActive(true);

            if (shapeName == "Triangle" && triangleHighlight != null)
                triangleHighlight.SetActive(true);

            currentStep++;

            if (currentStep >= correctOrder.Length)
            {
                levelCompleted = true;

                if (popupSuccess != null)
                    popupSuccess.SetActive(true);
            }
        }
        else
        {
            if (popupError != null)
                popupError.SetActive(true);
        }
    }

    public void CloseError()
    {
        if (popupError != null)
            popupError.SetActive(false);

        ResetLevel();
    }

    public void CloseSuccess()
    {
        
        if (popupSuccess != null)
            popupSuccess.SetActive(false);

        if (popupAdvice != null)
            popupAdvice.SetActive(true);
            
        OnAdvice.Invoke();
    }

    public void CloseAdvice()
    {
        if (popupAdvice != null)
            popupAdvice.SetActive(false);
        
        OnSuccess.Invoke();
        Debug.Log("Level 6 complete");
    }

    public void ResetLevel()
    {
        currentStep = 0;
        levelCompleted = false;

        if (circleHighlight != null) circleHighlight.SetActive(false);
        if (squareHighlight != null) squareHighlight.SetActive(false);
        if (triangleHighlight != null) triangleHighlight.SetActive(false);
    }
}
