using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonologueUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI monologueText;

    private Animator animator;

    private const string OPEN_TRIGGER = "Open";
    private const string CLOSE_TRIGGER = "Close";

    private const string PLAY_TRIGGER = "Play";
    private const string SKIP_TRIGGER = "Skip";

    private CanvasGroup canvasGroup;

    private void OnEnable()
    {
        MonologueManager.OnMonologueStart += MonologueManager_OnMonologueStart;
        MonologueManager.OnMonologueEnd += DialogueManager_OnDialogueEnd;
        MonologueManager.OnSentencePlay += MonologueManager_OnSentencePlay;
        MonologueManager.OnSentenceSkip += MonologueManager_OnSentenceSkip;
    }

    private void OnDisable()
    {
        MonologueManager.OnMonologueStart -= MonologueManager_OnMonologueStart;
        MonologueManager.OnMonologueEnd -= DialogueManager_OnDialogueEnd;
        MonologueManager.OnSentencePlay -= MonologueManager_OnSentencePlay;
        MonologueManager.OnSentenceSkip -= MonologueManager_OnSentenceSkip;
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
        SetMonologueText(sentence.text);
        SetMonologueTextColor(sentence.speaker.nameColor);
    }

    private void SetMonologueText(string text) => monologueText.text = text;
    private void SetMonologueTextColor(Color nameColor) => monologueText.color = nameColor;

    private void PlaySentence(Sentence sentence, bool isFirstSentence)
    {
        SetDialogueUI(sentence);
        animator.SetTrigger(isFirstSentence ? OPEN_TRIGGER : PLAY_TRIGGER);
    }

    private void SkipSentence(bool isLastSentence)
    {
        animator.SetTrigger(isLastSentence ? CLOSE_TRIGGER : SKIP_TRIGGER);
    }

    private void ResetAllTriggers()
    {
        animator.ResetTrigger(OPEN_TRIGGER);
        animator.ResetTrigger(CLOSE_TRIGGER);
        animator.ResetTrigger(PLAY_TRIGGER);
        animator.ResetTrigger(SKIP_TRIGGER);
    }


    #region DialogueManager Subscriptions
    private void MonologueManager_OnMonologueStart(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        EnableDialogueUI();
    }
    private void DialogueManager_OnDialogueEnd(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        DisableDialogueUI();
        ResetAllTriggers();
    }

    private void MonologueManager_OnSentencePlay(object sender, MonologueManager.OnSentencePlayEventArgs e)
    {
        PlaySentence(e.sentence, e.isFirstSentence);
    }
    private void MonologueManager_OnSentenceSkip(object sender, MonologueManager.OnSentenceSkipEventArgs e)
    {
        SkipSentence(e.isLastSentence);
    }
    #endregion
}
