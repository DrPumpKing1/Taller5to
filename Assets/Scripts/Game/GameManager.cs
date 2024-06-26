using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    [SerializeField] private State previousState;

    public enum State {OnGameplay, OnUI, OnForcedDialogue, OnRestrictedDialogue, OnFreeDialogue, OnMonologue, OnLost}

    public State GameState => state;

    private void OnEnable()
    {
        UIManager.OnUIActive += UIManager_OnUIActive;
        UIManager.OnUIInactive += UIManager_OnUIInactive;

        DialogueManager.OnDialogueStart += DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd += DialogueManager_OnDialogueEnd;

        MonologueManager.OnMonologueStart += MonologueManager_OnMonologueStart;
        MonologueManager.OnMonologueEnd += MonologueManager_OnMonologueEnd;

        BossObjectDestruction.OnBossAllProjectionGemsLocked += BossObjectDestruction_OnBossDestroyedAllObjects;
    }

    private void OnDisable()
    {
        UIManager.OnUIActive -= UIManager_OnUIActive;
        UIManager.OnUIInactive -= UIManager_OnUIInactive;

        DialogueManager.OnDialogueStart -= DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd -= DialogueManager_OnDialogueEnd;

        MonologueManager.OnMonologueStart -= MonologueManager_OnMonologueStart;
        MonologueManager.OnMonologueEnd -= MonologueManager_OnMonologueEnd;

        BossObjectDestruction.OnBossAllProjectionGemsLocked -= BossObjectDestruction_OnBossDestroyedAllObjects;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetGameState(State.OnGameplay);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetGameState(State state)
    {
        previousState = this.state;
        this.state = state;
    }

    private void SetPreviousState(State state)
    {
        previousState = state;
    }

    #region UIManager Subcriptions
    private void UIManager_OnUIActive(object sender, System.EventArgs e)
    {
        SetGameState(State.OnUI);
    }

    private void UIManager_OnUIInactive(object sender, System.EventArgs e)
    {
        SetGameState(previousState);
    }
    #endregion

    #region DialogueManagerSubscriptions
    private void DialogueManager_OnDialogueStart(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        switch (e.movementLimitType)
        {
            case MovementLimitType.FreeMovement:
                SetGameState(State.OnFreeDialogue);
                break;
            case MovementLimitType.RestrictedMovement:
                SetGameState(State.OnRestrictedDialogue);
                break;
            case MovementLimitType.ZeroMovement:
                SetGameState(State.OnForcedDialogue);
                break;
        }
    }

    private void DialogueManager_OnDialogueEnd(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        if (state != State.OnForcedDialogue && state != State.OnFreeDialogue && state != State.OnRestrictedDialogue)
        {
            SetPreviousState(State.OnGameplay);
            return;
        }

        SetGameState(State.OnGameplay);       
    }
    #endregion

    #region MonologueManager Susbcriptions
    private void MonologueManager_OnMonologueStart(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        SetGameState(State.OnMonologue);
    }
    private void MonologueManager_OnMonologueEnd(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        if(state != State.OnMonologue)
        {
            SetPreviousState(State.OnGameplay);
            return;
        }

        SetGameState(State.OnGameplay);
    }
    #endregion

    #region BossObjectDestruction Subscriptions
    private void BossObjectDestruction_OnBossDestroyedAllObjects(object sender, System.EventArgs e)
    {
        SetGameState(State.OnLost);
    }
    #endregion
}
