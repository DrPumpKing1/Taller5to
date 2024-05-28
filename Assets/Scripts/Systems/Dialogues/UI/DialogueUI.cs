using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI sentenceText;
    [SerializeField] private Image speakerImage;

    private Animator animator;

    private const string OPEN_TRIGGER = "Open";
    private const string CLOSE_TRIGGER = "Close";

    private const string PLAY_TRIGGER = "Play";
    private const string SKIP_TRIGGER = "Skip";

    private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        DialogueManager.OnDialogueStart += DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd += DialogueManager_OnDialogueEnd;
        DialogueManager.OnSentencePlay += DialogueManager_OnSentencePlay;
        DialogueManager.OnSentenceSkip += DialogueManager_OnSentenceSkip;
    }

    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

    }

    private void Start()
    {
        DisableDialogueUI();
    }

    private void EnableDialogueUI() => GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);
    private void DisableDialogueUI() => GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);

    private void SetDialogueUI(Sentence sentence)
    {
        SetSpeakerNameText(sentence.speaker.speakerName);
        SetSentenceText(sentence.text);
        SetSpeakerImage(sentence.speaker.speakerImage);
    }

    private void SetSpeakerNameText(string text) => speakerNameText.text = text;
    private void SetSentenceText(string text) => sentenceText.text = text;
    private void SetSpeakerImage(Sprite sprite) => speakerImage.sprite = sprite;

    private void PlaySentence(Sentence sentence, bool isFirstSentence)
    {
        SetDialogueUI(sentence);
        animator.SetTrigger(isFirstSentence? OPEN_TRIGGER:PLAY_TRIGGER);
    }

    private void SkipSentence(bool isLastSentence)
    {
        animator.SetTrigger(isLastSentence ? CLOSE_TRIGGER : SKIP_TRIGGER);
    }


    #region DialogueManager Subscriptions
    private void DialogueManager_OnDialogueStart(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        EnableDialogueUI();
    }
    private void DialogueManager_OnDialogueEnd(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        DisableDialogueUI();
    }

    private void DialogueManager_OnSentencePlay(object sender, DialogueManager.OnSentencePlayEventArgs e)
    {
        PlaySentence(e.sentence, e.isFirstSentence);
    }
    private void DialogueManager_OnSentenceSkip(object sender, DialogueManager.OnSentenceSkipEventArgs e)
    {
        SkipSentence(e.isLastSentence);
    }
    #endregion
}
