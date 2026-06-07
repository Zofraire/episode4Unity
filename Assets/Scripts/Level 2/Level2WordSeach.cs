using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Level2WordSearch : MonoBehaviour
{
    [Header("Found Words")]
    public bool foundBlock = false;
    public bool foundReport = false;

    [Header("Word Highlights In Grid")]
    public GameObject blockHighlight;
    public GameObject reportHighlight;

    [Header("Bottom Strike Lines")]
    public GameObject blockLine;
    public GameObject reportLine;

    [Header("Hotspots")]
    public Button blockHotspot;
    public Button reportHotspot;

    [Header("Popups")]
    public GameObject popupError;
    public GameObject popupSuccess;
    public GameObject popupAdvice;

    [Header("Advice Text")]
    public TMP_Text adviceText;
    
    public UnityEvent OnAdvice;
    public UnityEvent OnSuccess;

    private void Start()
    {
        ResetLevel();
    }

    public void SelectBlock()
    {
        if (foundBlock) return;

        foundBlock = true;
        Debug.Log("BLOCK CLICKED");

        if (blockHighlight != null)
            blockHighlight.SetActive(true);

        if (blockLine != null)
            blockLine.SetActive(true);

        if (blockHotspot != null)
            blockHotspot.interactable = false;
    }

    public void SelectReport()
    {
        if (foundReport) return;

        foundReport = true;
        Debug.Log("REPORT CLICKED");

        if (reportHighlight != null)
            reportHighlight.SetActive(true);

        if (reportLine != null)
            reportLine.SetActive(true);

        if (reportHotspot != null)
            reportHotspot.interactable = false;
    }

    public void CheckAnswer()
    {
        Debug.Log("Checking Level 2 answer");

        if (foundBlock && foundReport)
        {
            Debug.Log("LEVEL 2 SUCCESS");

            if (popupSuccess != null)
                popupSuccess.SetActive(true);
        }
        else
        {
            Debug.Log("LEVEL 2 ERROR");

            if (popupError != null)
                popupError.SetActive(true);
        }
    }

    public void CloseError()
    {
        if (popupError != null)
            popupError.SetActive(false);
    }

    public void CloseSuccess()
    {
        OnAdvice.Invoke();

        if (popupSuccess != null)
            popupSuccess.SetActive(false);

        if (popupAdvice != null)
            popupAdvice.SetActive(true);
    }

    public void CloseAdvice()
    {
        if (popupAdvice != null)
            popupAdvice.SetActive(false);
            
        OnSuccess.Invoke();

        Debug.Log("Level 2 complete");
    }

    public void ResetLevel()
    {
        foundBlock = false;
        foundReport = false;

        if (blockHighlight != null)
            blockHighlight.SetActive(false);

        if (reportHighlight != null)
            reportHighlight.SetActive(false);

        if (blockLine != null)
            blockLine.SetActive(false);

        if (reportLine != null)
            reportLine.SetActive(false);

        if (blockHotspot != null)
            blockHotspot.interactable = true;

        if (reportHotspot != null)
            reportHotspot.interactable = true;
    }
}
