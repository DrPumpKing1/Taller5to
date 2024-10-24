using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    [SerializeField] private State previousState;

    public enum State {OnGameplay, OnUI, OnCinematic, OnLost, OnWin}

    public State GameState => state;

    private void OnEnable()
    {
        UIManager.OnUIActive += UIManager_OnUIActive;
        UIManager.OnUIInactive += UIManager_OnUIInactive;

        CinematicsManager.OnCinematicStart += CinematicsManager_OnCinematicStart;
        CinematicsManager.OnCinematicEnd += CinematicsManager_OnCinematicEnd;

        LoseManager.OnLose += LoseManager_OnLose;
        WinManager.OnWin += WinManager_OnWin;
    }

    private void OnDisable()
    {
        UIManager.OnUIActive -= UIManager_OnUIActive;
        UIManager.OnUIInactive -= UIManager_OnUIInactive;

        CinematicsManager.OnCinematicStart -= CinematicsManager_OnCinematicStart;
        CinematicsManager.OnCinematicEnd -= CinematicsManager_OnCinematicEnd;

        LoseManager.OnLose -= LoseManager_OnLose;
        WinManager.OnWin -= WinManager_OnWin;
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

    #region CinematicsManager Subscriptions
    private void CinematicsManager_OnCinematicStart(object sender, CinematicsManager.OnCinematicEventArgs e)
    {
        SetGameState(State.OnCinematic);

    }

    private void CinematicsManager_OnCinematicEnd(object sender, CinematicsManager.OnCinematicEventArgs e)
    {
        SetGameState(State.OnGameplay);

    }
    #endregion

    #region LoseManager Subscriptions
    private void LoseManager_OnLose(object sender, System.EventArgs e)
    {
        SetGameState(State.OnLost);
    }
    #endregion

    #region WinManager Subscriptions
    private void WinManager_OnWin(object sender, System.EventArgs e)
    {
        SetGameState(State.OnWin);
    }
    #endregion
}
