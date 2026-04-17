using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Level3Phone : MonoBehaviour
{
    [Header("Panels")]
    public GameObject memorizePanel;
    public GameObject choicePanel;

    [Header("Texts")]
    public TMP_Text memorizeNumberText;
    public TMP_Text choiceNumberText;
    public TMP_Text adviceText;

    [Header("Correct Number")]
    public string correctNumber = "+976 88994519";

    [Header("Popups")]
    public GameObject popupError;
    public GameObject popupSuccess;
    public GameObject popupAdvice;

    public UnityEvent OnAdvice;
    public UnityEvent OnSuccess;

    void Start()
    {
        // Show memorize first
        memorizePanel.SetActive(true);
        choicePanel.SetActive(false);

        memorizeNumberText.text = correctNumber;
        choiceNumberText.text = correctNumber;
    }

    // NEXT BUTTON
    public void GoToChoice()
    {
        memorizePanel.SetActive(false);
        choicePanel.SetActive(true);
    }

    // GREEN BUTTON (correct)
    public void OnGreenClick()
    {
        Debug.Log("Correct choice");

        popupSuccess.SetActive(true);
    }

    // RED BUTTON (wrong)
    public void OnRedClick()
    {
        Debug.Log("Wrong choice");

        popupError.SetActive(true);
    }

    public void CloseError()
    {
        popupError.SetActive(false);
    }

    public void CloseSuccess()
    {
        popupSuccess.SetActive(false);

        adviceText.text = "Хэнтэй холбоо барьж буйгаа сайн нягтлаарай.";
        popupAdvice.SetActive(true);
        OnAdvice.Invoke();
    }

    public void CloseAdvice()
    {
        popupAdvice.SetActive(false);
        OnSuccess.Invoke();
        Debug.Log("Level 3 Complete!");
    }
}
