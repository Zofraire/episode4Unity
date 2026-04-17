using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class DragDropUIManager : MonoBehaviour
{
    [Header("Timeline")]
    public PlayableDirector director;
    public TimelineAsset introTimeline;
    public TimelineAsset finalTimeline;
    public TimelineAsset[] correctTimelines;

    [Header("End Game Activation")]
    public GameObject endWindow; // The game object to enable at the end
    public float endDelay = 3.5f; // 3-4 second delay as requested

    [Header("Audio")]
    public AudioClip wrongAudio;
    public AudioClip[] itemAudioClips;
    private AudioSource audioSource;

    [Header("Draggables")]
    public GameObject[] draggableItems; 
    public string[] draggableTexts;

    [Header("DropZones")]
    public DropZoneHandler[] dropZones; 

    [Header("Correct Order")]
    public int[] correctOrder; 

    [Header("Message Boxes")]
    public GameObject correctMessageBox;
    public GameObject incorrectMessageBox;

    private int currentIndex = 0;
    private bool timelinePlaying = false;
    [HideInInspector] public bool interactable = true;

    private DropZoneSnapshot[] roundSnapshots;

    [System.Serializable]
    public class DropZoneSnapshot
    {
        public string[] texts;
        public bool[] active;

        public DropZoneSnapshot(int count)
        {
            texts = new string[count];
            active = new bool[count];
        }

        public void Save(Image[] boxes, TMP_Text[] textsArray)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                active[i] = boxes[i].gameObject.activeSelf;
                texts[i] = textsArray[i].text;
            }
        }

        public void Restore(Image[] boxes, TMP_Text[] textsArray)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].gameObject.SetActive(active[i]);
                textsArray[i].gameObject.SetActive(active[i]);
                textsArray[i].text = texts[i];
            }
        }
    }

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        if (correctMessageBox == null)
            correctMessageBox = GameObject.Find("CorrectMessageBox");
        if (incorrectMessageBox == null)
            incorrectMessageBox = GameObject.Find("IncorrectMessageBox");

        if (correctMessageBox != null) correctMessageBox.SetActive(false);
        if (incorrectMessageBox != null) incorrectMessageBox.SetActive(false);
        if (endWindow != null) endWindow.SetActive(false); // Ensure hidden at start

        for (int i = 0; i < draggableItems.Length; i++)
        {
            var drag = draggableItems[i].AddComponent<DragItem>();
            drag.manager = this;
            drag.index = i;
            drag.itemText = draggableTexts[i];
            draggableItems[i].SetActive(false);
        }

        foreach (var dz in dropZones)
        {
            dz.Setup();
        }

        StartCoroutine(PlayIntroTimeline());
    }

    IEnumerator PlayIntroTimeline()
    {
        timelinePlaying = true;
        if (introTimeline != null)
        {
            director.playableAsset = introTimeline;
            director.Play();
            while (director.state == PlayState.Playing)
                yield return null;
        }

        ActivateCurrentDraggable();
        timelinePlaying = false;
    }

    void ActivateCurrentDraggable()
    {
        if (currentIndex >= draggableItems.Length) return;
        draggableItems[currentIndex].SetActive(true);

        if (itemAudioClips != null && currentIndex < itemAudioClips.Length && itemAudioClips[currentIndex] != null)
        {
            StartCoroutine(PlayAudioAndLock(itemAudioClips[currentIndex]));
        }

        SaveCurrentRoundState();
    }

    IEnumerator PlayAudioAndLock(AudioClip clip)
    {
        interactable = false;
        audioSource.PlayOneShot(clip);
        while (audioSource.isPlaying)
            yield return null;
        interactable = true;
    }

    void SaveCurrentRoundState()
    {
        roundSnapshots = new DropZoneSnapshot[dropZones.Length];
        for (int i = 0; i < dropZones.Length; i++)
        {
            var dz = dropZones[i];
            roundSnapshots[i] = new DropZoneSnapshot(dz.boxImages.Length);
            roundSnapshots[i].Save(dz.boxImages, dz.boxTexts);
        }
    }

    public void OnItemDropped(int itemIndex, int dropZoneIndex)
    {
        if (timelinePlaying || !interactable) return;

        var dz = dropZones[dropZoneIndex];
        TMP_Text targetText = dz.GetNextAvailableText();
        Image targetBox = dz.GetNextAvailableImage();

        if (targetText != null && targetBox != null)
        {
            targetText.text = draggableTexts[itemIndex];
            targetBox.gameObject.SetActive(true);
            targetText.gameObject.SetActive(true);
        }

        int expectedDropZone = correctOrder[currentIndex];

        if (dropZoneIndex == expectedDropZone)
        {
            draggableItems[currentIndex].SetActive(false);
            if (correctMessageBox != null)
                correctMessageBox.SetActive(true);
        }
        else
        {
            draggableItems[currentIndex].SetActive(false);
            if (incorrectMessageBox != null)
                incorrectMessageBox.SetActive(true);

            if (wrongAudio != null)
                audioSource.PlayOneShot(wrongAudio);
        }
    }

    public void OnCorrectOK()
    {
        if (correctMessageBox != null)
            correctMessageBox.SetActive(false);
        StartCoroutine(PlayCorrectTimeline(currentIndex));
    }

    public void OnIncorrectOK()
    {
        if (incorrectMessageBox != null)
            incorrectMessageBox.SetActive(false);

        for (int i = 0; i < dropZones.Length; i++)
        {
            roundSnapshots[i].Restore(dropZones[i].boxImages, dropZones[i].boxTexts);
            dropZones[i].nextBoxIndex = 0;
            for (int j = 0; j < dropZones[i].boxImages.Length; j++)
            {
                if (dropZones[i].boxImages[j].gameObject.activeSelf)
                    dropZones[i].nextBoxIndex++;
            }
        }

        draggableItems[currentIndex].SetActive(true);
        if (itemAudioClips != null && currentIndex < itemAudioClips.Length && itemAudioClips[currentIndex] != null)
        {
            StartCoroutine(PlayAudioAndLock(itemAudioClips[currentIndex]));
        }
    }

    IEnumerator PlayCorrectTimeline(int itemIndex)
    {
        timelinePlaying = true;

        if (correctTimelines != null && itemIndex < correctTimelines.Length)
        {
            var tl = correctTimelines[itemIndex];
            if (tl != null)
            {
                director.playableAsset = tl;
                director.Play();
                while (director.state == PlayState.Playing)
                    yield return null;
            }
        }

        currentIndex++;

        if (currentIndex >= draggableItems.Length)
        {
            // All items finished - play final cinematic
            if (finalTimeline != null)
            {
                director.playableAsset = finalTimeline;
                director.Play();
                while (director.state == PlayState.Playing)
                    yield return null;
            }

            // --- TRIGGER ENDING SEQUENCE ---
            StartCoroutine(ActivateEndWindowAfterDelay());
        }
        else
        {
            ActivateCurrentDraggable();
            timelinePlaying = false;
        }
    }

    // New coroutine to handle the 3-4 second window
    IEnumerator ActivateEndWindowAfterDelay()
    {
        yield return new WaitForSeconds(endDelay);
        
        if (endWindow != null)
        {
            endWindow.SetActive(true);
            
            // --- CHANGE THE NUMBER HERE ---
            // If this is Game 2, and you want to unlock Game 3, set this to 3.
            int nextLevel = 3; 

            int currentProgress = PlayerPrefs.GetInt("levelsUnlocked", 1);
            if (currentProgress < nextLevel) 
            {
                PlayerPrefs.SetInt("levelsUnlocked", nextLevel); 
                PlayerPrefs.Save();
                Debug.Log("Level " + nextLevel + " Unlocked!");
            }
        }
    }

    // ---------------- Drag Item Inner Class ----------------
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public DragDropUIManager manager;
        public int index;
        public string itemText;

        private Vector3 startPos;
        private RectTransform rect;
        private CanvasGroup canvasGroup;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!manager.interactable) return;
            startPos = rect.position;
            canvasGroup.blocksRaycasts = false;
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!manager.interactable) return;
            rect.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!manager.interactable) return;
            rect.position = startPos;
            canvasGroup.blocksRaycasts = true;

            if (eventData.pointerEnter != null)
            {
                var dz = eventData.pointerEnter.GetComponent<DropZoneHandler>();
                if (dz != null)
                    manager.OnItemDropped(index, dz.dropZoneIndex);
            }
        }
    }
}