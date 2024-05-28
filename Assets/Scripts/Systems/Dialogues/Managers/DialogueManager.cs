using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float timeBetweenSentences;
    [SerializeField] private float timeToEndAfterLastSentenceSkip;

    [Header("States")]
    [SerializeField] private State state;

    private enum State{ NotOnDialogue, OnSentence, SkippingSentence }

    public static event EventHandler<OnDialogueEventArgs> OnDialogueStart;
    public static event EventHandler<OnDialogueEventArgs> OnDialogueEnd;

    public static event EventHandler<OnSentencePlayEventArgs> OnSentencePlay;
    public static event EventHandler<OnSentenceSkipEventArgs> OnSentenceSkip;

    public class OnDialogueEventArgs : EventArgs
    {
        public DialogueSO dialogueSO;
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

    public void StartDialogue(DialogueSO dialogueSO)
    {

    }
}
