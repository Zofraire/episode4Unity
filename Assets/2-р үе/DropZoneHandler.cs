using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DropZoneHandler : MonoBehaviour
{
    [HideInInspector] public int dropZoneIndex;
    [HideInInspector] public int nextBoxIndex = 0;

    [Header("Boxes (Image -> TMP_Text)")]
    public Image[] boxImages;
    public TMP_Text[] boxTexts;

    public void Setup()
    {
        nextBoxIndex = 0;
        for (int i = 0; i < boxImages.Length; i++)
        {
            boxImages[i].gameObject.SetActive(false);
            boxTexts[i].gameObject.SetActive(false);
            boxTexts[i].text = "";
        }
    }

    public TMP_Text GetNextAvailableText()
    {
        if (nextBoxIndex < boxTexts.Length)
            return boxTexts[nextBoxIndex];
        return null;
    }

    public Image GetNextAvailableImage()
    {
        if (nextBoxIndex < boxImages.Length)
            return boxImages[nextBoxIndex++];
        return null;
    }
}