using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;
using static PetGuidanceListener;
using static CinematicsManager;

public class CinematicsManager : MonoBehaviour
{
    public static CinematicsManager Instance { get; private set; }

    [Header("Cinematics")]
    [SerializeField] private List<Cinematic> cinematics;

    [Serializable]
    public class Cinematic
    {
        public int id;
        public string logToPlay;
        public VideoClip videoClip;
    }

    private Cinematic currentCinematic;

    public static event EventHandler<OnCinematicEventArgs> OnCinematicStart;
    public static event EventHandler<OnCinematicEventArgs> OnCinematicEnd;
    public class OnCinematicEventArgs : EventArgs
    {
        public Cinematic cinematic;
    }

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
        CinematicsUIHandler.OnCinematicUIEnd += CinematicsUIHandler_OnCinematicUIEnd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
        CinematicsUIHandler.OnCinematicUIEnd -= CinematicsUIHandler_OnCinematicUIEnd;

    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CinematicsManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void CheckStartCinematic(string log)
    {
        foreach (Cinematic cinematic in cinematics)
        {
            if (cinematic.logToPlay == log)
            {
                StartCinematic(cinematic);
                return;
            }
        }
    }

    private void StartCinematic(Cinematic cinematic)
    {
        SetCurrentCinematic(cinematic); 
        OnCinematicStart?.Invoke(this, new OnCinematicEventArgs { cinematic = cinematic });
    }

    private void EndCinematic()
    {
        OnCinematicEnd?.Invoke(this, new OnCinematicEventArgs { cinematic = currentCinematic });
        ClearCurrentCinematic();
    }

    private void SetCurrentCinematic(Cinematic cinematic) =>  currentCinematic = cinematic;
    private void ClearCurrentCinematic() => currentCinematic = null;


    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckStartCinematic(e.gameplayAction.log);
    }
    #endregion

    #region CinematicsUIHandlerSubscriptions
    private void CinematicsUIHandler_OnCinematicUIEnd(object sender, EventArgs e)
    {
        EndCinematic();
    }
    #endregion
}
