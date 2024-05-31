using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private DialogueInput dialogueInput;

    [Header("Settings")]
    [SerializeField] private float timeBetweenSentences;
    [SerializeField] private float timeToStartDialogue;
    [SerializeField] private float timeToEndDialogue;
    [SerializeField] private float timeToPlaySentence;
    [SerializeField] private float timeToSkipSentence;
    [Space]
    [SerializeField] private bool enableDialogueSkip;
    [SerializeField] private bool enableDialoguesAutoSkipByTime;

    [Header("States")]
    [SerializeField] private State state;

    private enum State{ NotOnDialogue, OnSentence, BetweenSentences, StartingDialogue, EndingDialogue, PlayingSentence, SkippingSentence }

    private bool SkipInput => dialogueInput.GetSkipDown();

    public static event EventHandler<OnDialogueEventArgs> OnDialogueStart;
    public static event EventHandler<OnDialogueEventArgs> OnDialogueEnd;

    public static event EventHandler<OnSentencePlayEventArgs> OnSentencePlay;
    public static event EventHandler<OnSentenceSkipEventArgs> OnSentenceSkip;

    private DialogueSO currentDialogueSO;
    private int currentSentenceIndex;
    private Sentence currentSentence;

    private float timer;

    public class OnDialogueEventArgs : EventArgs
    {
        public DialogueSO dialogueSO;
        public bool limitMovement;
    }

    public class OnSentencePlayEventArgs : EventArgs
    {
        public Sentence sentence;
        public bool isFirstSentence;
    }

    public class OnSentenceSkipEventArgs : EventArgs
    {
        public Sentence sentence;
        public bool isLastSentence;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetDialogueState(State.NotOnDialogue);
        ResetTimer();
    }

    private void Update()
    {
        HandleDialogueStates();
    }
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one DialogueManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    private void SetDialogueState(State state) => this.state = state;

    private void HandleDialogueStates()
    {
        switch (state)
        {
            case State.NotOnDialogue:
                NotOnDialogueLogic();
                break;
            case State.OnSentence:
                OnSentenceLogic();
                break;
            case State.BetweenSentences:
                BetweenSentencesLogic();
                break;
            case State.StartingDialogue:
                StartingDialogueLogic();
                break;
            case State.EndingDialogue:
                EndingDialogueLogic();
                break;
            case State.PlayingSentence:
                PlayingSentenceLogic();
                break;
            case State.SkippingSentence:
                SkippingSentenceLogic();
                break;
        }
    }

    private void NotOnDialogueLogic() { }

    private void OnSentenceLogic()
    {
        if (timer < currentSentence.time)
        {
            timer += Time.deltaTime;
        }
        else if (enableDialoguesAutoSkipByTime)
        {
            SkipSentence();
            return;
        }

        if (CheckSkipSentence() && enableDialogueSkip)
        {
            SkipSentence();
            return;
        }       
    }

    private void SkipSentence()
    {
        ResetTimer();

        if (CheckCurrentSentenceIsLastSentence())
        {
            SetDialogueState(State.EndingDialogue);
            OnSentenceSkip?.Invoke(this, new OnSentenceSkipEventArgs { sentence = currentSentence, isLastSentence = true });
        }
        else
        {
            SetDialogueState(State.SkippingSentence);
            OnSentenceSkip?.Invoke(this, new OnSentenceSkipEventArgs { sentence = currentSentence, isLastSentence = false });
        }
    }

    private void BetweenSentencesLogic()
    {
        if (timer < timeBetweenSentences)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ResetTimer();
            SetCurrentSentence(currentSentenceIndex+1);
            OnSentencePlay?.Invoke(this , new OnSentencePlayEventArgs { sentence = currentSentence, isFirstSentence = false });
            SetDialogueState(State.PlayingSentence);
        }
    }

    private void StartingDialogueLogic()
    {
        if (timer < timeToStartDialogue)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ResetTimer();
            SetDialogueState(State.OnSentence);
        }
    }

    private void EndingDialogueLogic()
    {
        if (timer < timeToEndDialogue)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ResetTimer();
            OnDialogueEnd?.Invoke(this, new OnDialogueEventArgs { dialogueSO = currentDialogueSO, limitMovement = currentDialogueSO.limitMovement });
            ClearVariables();
            SetDialogueState(State.NotOnDialogue);
        }
    }

    private void PlayingSentenceLogic()
    {
        if (timer < timeToPlaySentence)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ResetTimer();
            SetDialogueState(State.OnSentence);
        }
    }

    private void SkippingSentenceLogic()
    {
        if (timer < timeToSkipSentence)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ResetTimer();
            SetDialogueState(State.BetweenSentences);
        }
    }

    private bool CheckSkipSentence()
    {
        if (SkipInput)
        {
            return true;
        }
        return false;
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        if (state != State.NotOnDialogue) return;
        if (dialogueSO.sentences.Count == 0) return;

        ResetTimer();

        currentDialogueSO = dialogueSO;
        SetCurrentSentence(0);

        OnDialogueStart?.Invoke(this, new OnDialogueEventArgs { dialogueSO = currentDialogueSO, limitMovement = currentDialogueSO.limitMovement});
        OnSentencePlay?.Invoke(this, new OnSentencePlayEventArgs { sentence = currentSentence, isFirstSentence = true });

        SetDialogueState(State.StartingDialogue);
    }

    public void EndDialogue()
    {
        if (state == State.NotOnDialogue) return;
        if (state == State.EndingDialogue) return;

        ResetTimer();
        SetDialogueState(State.EndingDialogue);
        OnSentenceSkip?.Invoke(this, new OnSentenceSkipEventArgs { sentence = currentSentence, isLastSentence = true });
    }

    private void SetCurrentSentence(int index)
    {
        currentSentenceIndex = index;
        currentSentence = currentDialogueSO.sentences[index];
    }

    private bool CheckCurrentSentenceIsLastSentence()
    {
        if (currentSentence == currentDialogueSO.sentences[^1]) return true;
        return false;
    }
    private void ResetTimer() => timer = 0f;

    private void ClearVariables()
    {
        currentDialogueSO = null;
        currentSentence = null;
        currentSentenceIndex = 0;
    }
}
