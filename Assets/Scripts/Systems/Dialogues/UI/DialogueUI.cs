using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI sentenceText;
    [SerializeField] private Image speakerImage;
    [SerializeField] private Image speakerBoxImage;
    [SerializeField] private Image sentenceBoxImage;

    [Header("Dialogue Type Sprites")]
    [SerializeField] private Sprite speakerBoxSpriteMechanicsDialogue;
    [SerializeField] private Sprite sentenceBoxSpriteMechanicsDialogue;
    [Space]
    [SerializeField] private Sprite speakerBoxSpriteNarrativeDialogue;
    [SerializeField] private Sprite sentenceBoxSpriteNarrativeDialogue;

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
    private void OnDisable()
    {
        DialogueManager.OnDialogueStart -= DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd -= DialogueManager_OnDialogueEnd;
        DialogueManager.OnSentencePlay -= DialogueManager_OnSentencePlay;
        DialogueManager.OnSentenceSkip -= DialogueManager_OnSentenceSkip;
    }


    private void Awake()
    {
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
        SetSpeakerTextColor(sentence.speaker.nameColor);
        SetSpeakerImage(sentence.speaker.speakerImage);
        SetSentenceText(sentence.text);
    }

    private void SetSpeakerNameText(string text) => speakerNameText.text = text;
    private void SetSpeakerTextColor(Color nameColor) => speakerNameText.color = nameColor;
    private void SetSpeakerImage(Sprite sprite) => speakerImage.sprite = sprite;
    private void SetSentenceText(string text) => sentenceText.text = text;

    private void SetSpeakerBoxSprite(Sprite sprite) => speakerBoxImage.sprite = sprite;
    private void SetSentenceBoxSprite(Sprite sprite) => sentenceBoxImage.sprite = sprite;

    private void SetDialogueSprites(DialogueType dialogueType)
    {
        switch (dialogueType)
        {
            case DialogueType.Mechanics:
            default:
                SetSpeakerBoxSprite(speakerBoxSpriteMechanicsDialogue);
                SetSentenceBoxSprite(sentenceBoxSpriteMechanicsDialogue);
                break;
            case DialogueType.Narrative:
                SetSpeakerBoxSprite(speakerBoxSpriteNarrativeDialogue);
                SetSentenceBoxSprite(sentenceBoxSpriteNarrativeDialogue);
                break;
        }
    }


    private void PlaySentence(Sentence sentence, bool isFirstSentence)
    {
        SetDialogueUI(sentence);
        animator.SetTrigger(isFirstSentence? OPEN_TRIGGER : PLAY_TRIGGER);
    }

    private void SkipSentence(bool isLastSentence)
    {
        animator.SetTrigger(isLastSentence ? CLOSE_TRIGGER : SKIP_TRIGGER);
    }

    private void ResetShowTriggers()
    {
        animator.ResetTrigger(OPEN_TRIGGER);
        animator.ResetTrigger(PLAY_TRIGGER);
    }

    private void ResetHideTriggers()
    {
        animator.ResetTrigger(CLOSE_TRIGGER);
        animator.ResetTrigger(SKIP_TRIGGER);
    }


    #region DialogueManager Subscriptions
    private void DialogueManager_OnDialogueStart(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        ResetHideTriggers();
        SetDialogueSprites(e.dialogueType);
        EnableDialogueUI();
    }
    private void DialogueManager_OnDialogueEnd(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        DisableDialogueUI();
        ResetShowTriggers();
    }

    private void DialogueManager_OnSentencePlay(object sender, DialogueManager.OnSentencePlayEventArgs e)
    {
        ResetHideTriggers();
        PlaySentence(e.sentence, e.isFirstSentence);
    }

    private void DialogueManager_OnSentenceSkip(object sender, DialogueManager.OnSentenceSkipEventArgs e)
    {
        ResetShowTriggers();
        SkipSentence(e.isLastSentence);
    }
    #endregion
}
