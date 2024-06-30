using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonologueManager : MonoBehaviour
{
    public static MonologueManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float timeBetweenSentences;
    [SerializeField] private float timeToStartMonologue;
    [SerializeField] private float timeToEndMonologue;
    [SerializeField] private float timeToPlayMonologue;
    [SerializeField] private float timeToSkipSentence;

    [Header("States")]
    [SerializeField] private State state;
    [SerializeField] private ManagerState managerState;

    public enum ManagerState { NotOnMonologue, OnMonologue}
    private enum State { NotOnMonologue, OnMonologue, BetweenSentences, StartingMonologue, EndingMonologue, PlayingSentence, SkippingSentence }

    public ManagerState _ManagerState => managerState;

    public static event EventHandler<OnMonologueEventArgs> OnMonologueStart;
    public static event EventHandler<OnMonologueEventArgs> OnMonologueEnd;

    public static event EventHandler<OnSentencePlayEventArgs> OnSentencePlay;
    public static event EventHandler<OnSentenceSkipEventArgs> OnSentenceSkip;

    private MonologueSO currentMonologueSO;
    private int currentSentenceIndex;
    private Sentence currentSentence;

    private float timer;

    public class OnMonologueEventArgs : EventArgs
    {
        public MonologueSO monologueSO;
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
        SetMonologueState(State.NotOnMonologue);
        SetMonologueManagerState(ManagerState.NotOnMonologue);
        ResetTimer();
    }

    private void Update()
    {
        HandleMonologueStates();
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
            Debug.LogWarning("There is more than one MonologueManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    private void SetMonologueState(State state) => this.state = state;
    private void SetMonologueManagerState(ManagerState state) => managerState = state;

    private void HandleMonologueStates()
    {
        switch (state)
        {
            case State.NotOnMonologue:
                NotOnMonologueLogic();
                break;
            case State.OnMonologue:
                OnSentenceLogic();
                break;
            case State.BetweenSentences:
                BetweenSentencesLogic();
                break;
            case State.StartingMonologue:
                StartingMonologueLogic();
                break;
            case State.EndingMonologue:
                EndingMonologueLogic();
                break;
            case State.PlayingSentence:
                PlayingSentenceLogic();
                break;
            case State.SkippingSentence:
                SkippingSentenceLogic();
                break;
        }
    }

    private void NotOnMonologueLogic() { }

    private void OnSentenceLogic()
    {
        if (timer < currentSentence.time)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SkipSentence(); 
        }
    }

    private void SkipSentence()
    {
        ResetTimer();

        if (CheckCurrentSentenceIsLastSentence())
        {
            SetMonologueState(State.EndingMonologue);
            OnSentenceSkip?.Invoke(this, new OnSentenceSkipEventArgs { sentence = currentSentence, isLastSentence = true });
        }
        else
        {
            SetMonologueState(State.SkippingSentence);
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
            SetCurrentSentence(currentSentenceIndex + 1);
            OnSentencePlay?.Invoke(this, new OnSentencePlayEventArgs { sentence = currentSentence, isFirstSentence = false });
            SetMonologueState(State.PlayingSentence);
        }
    }

    private void StartingMonologueLogic()
    {
        if (timer < timeToStartMonologue)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ResetTimer();
            SetMonologueState(State.OnMonologue);
        }
    }

    private void EndingMonologueLogic()
    {
        if (timer < timeToEndMonologue)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ResetTimer();
            OnMonologueEnd?.Invoke(this, new OnMonologueEventArgs { monologueSO = currentMonologueSO });
            ClearVariables();
            SetMonologueState(State.NotOnMonologue);
            SetMonologueManagerState(ManagerState.NotOnMonologue);
        }
    }

    private void PlayingSentenceLogic()
    {
        if (timer < timeToPlayMonologue)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ResetTimer();
            SetMonologueState(State.OnMonologue);
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
            SetMonologueState(State.BetweenSentences);
        }
    }

    public void StartMonologue(MonologueSO monologueSO)
    {
        //if (state != State.NotOnMonologue) return;
        if (monologueSO.sentences.Count == 0) return;

        ResetTimer();

        currentMonologueSO = monologueSO;
        SetCurrentSentence(0);

        OnMonologueStart?.Invoke(this, new OnMonologueEventArgs { monologueSO = currentMonologueSO });
        OnSentencePlay?.Invoke(this, new OnSentencePlayEventArgs { sentence = currentSentence, isFirstSentence = true });

        SetMonologueState(State.StartingMonologue);
        SetMonologueManagerState(ManagerState.OnMonologue);
    }

    public void EndMonologue()
    {
        if (state == State.NotOnMonologue) return;
        //if (state == State.EndingMonologue) return;

        ResetTimer();
        SetMonologueState(State.EndingMonologue);
        OnSentenceSkip?.Invoke(this, new OnSentenceSkipEventArgs { sentence = currentSentence, isLastSentence = true });
    }

    private void SetCurrentSentence(int index)
    {
        currentSentenceIndex = index;
        currentSentence = currentMonologueSO.sentences[index];
    }

    private bool CheckCurrentSentenceIsLastSentence()
    {
        if (currentSentence == currentMonologueSO.sentences[^1]) return true;
        return false;
    }
    private void ResetTimer() => timer = 0f;

    private void ClearVariables()
    {
        currentMonologueSO = null;
        currentSentence = null;
        currentSentenceIndex = 0;
    }

    public bool PlayingMonologue()
    {
        if (state == State.NotOnMonologue) return false;
        return true;
    }
}
