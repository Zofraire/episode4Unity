using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Collections;

public class QuizGameController : MonoBehaviour
{
    [Header("Director")]
    public PlayableDirector director;

    [Header("Main Question Container")]
    public GameObject questionsRoot;

    [Header("Plane (Background / Overlay)")]
    public GameObject plane; 

    [Header("Timelines")]
    public PlayableAsset introTimeline;
    public PlayableAsset[] questionTimelines;
    public PlayableAsset[] correctTimelines;
    public PlayableAsset finalTimeline;

    [Header("Answers (0=A,1=B,2=C,3=D)")]
    public int[] correctAnswers;

    [Header("End Game Logic")]
    public GameObject objectToEnableAtEnd; 
    public float delayBeforeEnabling = 3.5f;

    [System.Serializable]
    public class Question
    {
        public GameObject root;

        public Button A;
        public Button B;
        public Button C;
        public Button D;

        public Image A_img;
        public Image B_img;
        public Image C_img;
        public Image D_img;
    }

    [Header("Questions")]
    public Question[] questions;

    [Header("Feedback")]
    public AudioSource wrongAudio;
    public Color normalColor = Color.white;
    public Color wrongColor = Color.red;
    public Color correctColor = Color.green;

    [Header("Timing")]
    public float wrongFlashTime = 0.4f;
    public float correctDelay = 0.4f;

    private int currentQuestion = 0;
    private bool canAnswer = false;

    void Start()
    {
        SetupButtons();
        HideAllQuestions();

        questionsRoot.SetActive(false);
        if (plane != null) plane.SetActive(false);
        if (objectToEnableAtEnd != null) objectToEnableAtEnd.SetActive(false);

        StartCoroutine(GameFlow());
    }

    void SetupButtons()
    {
        for (int i = 0; i < questions.Length; i++)
        {
            int q = i;
            questions[q].A.onClick.AddListener(() => SelectAnswer(q, 0));
            questions[q].B.onClick.AddListener(() => SelectAnswer(q, 1));
            questions[q].C.onClick.AddListener(() => SelectAnswer(q, 2));
            questions[q].D.onClick.AddListener(() => SelectAnswer(q, 3));
        }
    }

    IEnumerator GameFlow()
    {
        // 🎬 Intro
        yield return PlayAndWait(introTimeline);

        questionsRoot.SetActive(true);
        if (plane != null) plane.SetActive(true);

        for (currentQuestion = 0; currentQuestion < questions.Length; currentQuestion++)
        {
            Show(currentQuestion);
            yield return PlayAndWait(questionTimelines[currentQuestion]);

            SetInteractable(true);
            canAnswer = true;

            yield return new WaitUntil(() => canAnswer == false);

            SetInteractable(false);
            yield return PlayAndWait(correctTimelines[currentQuestion]);
            Hide(currentQuestion);
        }

        // Logic for the specific object activation after last question
        if (objectToEnableAtEnd != null)
        {
            StartCoroutine(EnableObjectAfterDelay());
        }

        // Disable UI
        questionsRoot.SetActive(false);
        if (plane != null) plane.SetActive(false);

        // 🏁 Final Timeline
        yield return PlayAndWait(finalTimeline);
    }

    IEnumerator EnableObjectAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeEnabling);
        int currentProgress = PlayerPrefs.GetInt("levelsUnlocked", 1);
        if (currentProgress < 2) 
        {
            PlayerPrefs.SetInt("levelsUnlocked", 2); 
            PlayerPrefs.Save();
            Debug.Log("Level 2 Unlocked!");
        }

        if (objectToEnableAtEnd != null)
        {
            objectToEnableAtEnd.SetActive(true);
        }
    }

    void SelectAnswer(int q, int answer)
    {
        if (!canAnswer || q != currentQuestion)
            return;

        if (answer == correctAnswers[currentQuestion])
        {
            canAnswer = false;
            SetInteractable(false);
            GetImage(q, answer).color = correctColor;
            StartCoroutine(CorrectSequence(q));
        }
        else
        {
            StartCoroutine(WrongFeedback(q, answer));
        }
    }

    IEnumerator CorrectSequence(int q)
    {
        yield return new WaitForSeconds(correctDelay);
        questions[q].root.SetActive(false);
    }

    IEnumerator WrongFeedback(int q, int answer)
    {
        if (wrongAudio != null) wrongAudio.Play();
        Image img = GetImage(q, answer);
        img.color = wrongColor;
        yield return new WaitForSeconds(wrongFlashTime);
        img.color = normalColor;
    }

    Image GetImage(int q, int answer)
    {
        switch (answer)
        {
            case 0: return questions[q].A_img;
            case 1: return questions[q].B_img;
            case 2: return questions[q].C_img;
            case 3: return questions[q].D_img;
            default: return null;
        }
    }

    void SetInteractable(bool state)
    {
        Question q = questions[currentQuestion];
        q.A.interactable = state;
        q.B.interactable = state;
        q.C.interactable = state;
        q.D.interactable = state;
    }

    void Show(int i) => questions[i].root.SetActive(true);
    void Hide(int i) => questions[i].root.SetActive(false);

    void HideAllQuestions()
    {
        foreach (var q in questions)
            q.root.SetActive(false);
    }

    IEnumerator PlayAndWait(PlayableAsset timeline)
    {
        if (timeline == null) yield break;
        director.playableAsset = timeline;
        director.Play();
        yield return new WaitForSeconds((float)timeline.duration);
    }
}