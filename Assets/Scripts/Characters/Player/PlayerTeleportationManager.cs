using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportationManager : MonoBehaviour
{
    public static PlayerTeleportationManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private TeleportationState state;

    public TeleportationState State => state;

    public enum TeleportationState { NotTeleporting, StallingStart, StartingTeleportation, EndingTelelportation, StallingEnd };

    public static event EventHandler<OnTeleportationEventArgs> OnTeleportationStallStarted;
    public static event EventHandler<OnTeleportationEventArgs> OnTeleportationStarted;
    public static event EventHandler<OnTeleportationEventArgs> OnTeleportationCompleted;
    public static event EventHandler<OnTeleportationEventArgs> OnTeleportationEnded;
    public static event EventHandler<OnTeleportationEventArgs> OnTeleportationStallEnded;

    public class OnTeleportationEventArgs : EventArgs
    {
        public TeleportationSetting teleportationSetting;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetTeleportationState(TeleportationState.NotTeleporting);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one PlayerTeleportationManager, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetTeleportationState(TeleportationState state) => this.state = state;

    public void StartTeleportation(TeleportationSetting teleportationSetting)
    {
        if (state != TeleportationState.NotTeleporting) return;

        StopAllCoroutines();
        StartCoroutine(TeleportPlayerCoroutine(teleportationSetting));
    }

    public bool AllowMovementInputProcessing() => state == TeleportationState.NotTeleporting;
    public bool AllowInteractionInputProcessing() => state == TeleportationState.NotTeleporting; 
    public bool AllowProjectionInputProcessing() => state == TeleportationState.NotTeleporting; 
    public bool AllowInventoryInputProcessing() => state == TeleportationState.NotTeleporting;
    public bool AllowJournalInputProcessing() => state == TeleportationState.NotTeleporting;

    private IEnumerator TeleportPlayerCoroutine(TeleportationSetting teleportationSetting)
    {
        SetTeleportationState(TeleportationState.StallingStart);

        OnTeleportationStallStarted?.Invoke(this, new OnTeleportationEventArgs { teleportationSetting = teleportationSetting });

        yield return new WaitForSeconds(teleportationSetting.stallStartTeleportationTime);

        SetTeleportationState(TeleportationState.StartingTeleportation);

        OnTeleportationStarted?.Invoke(this, new OnTeleportationEventArgs { teleportationSetting = teleportationSetting });

        yield return new WaitForSeconds(teleportationSetting.startingTeleportationTime);

        PlayerPositioningHandler.Instance.InstantPositionPlayer(teleportationSetting.teleportPosition.position);

        OnTeleportationCompleted?.Invoke(this, new OnTeleportationEventArgs { teleportationSetting = teleportationSetting });

        SetTeleportationState(TeleportationState.EndingTelelportation);

        yield return new WaitForSeconds(teleportationSetting.endingTeleportationTime);

        OnTeleportationEnded?.Invoke(this, new OnTeleportationEventArgs { teleportationSetting = teleportationSetting });

        SetTeleportationState(TeleportationState.StallingEnd);

        yield return new WaitForSeconds(teleportationSetting.stallEndTeleportationTime);

        OnTeleportationStallEnded?.Invoke(this, new OnTeleportationEventArgs { teleportationSetting = teleportationSetting });
        
        SetTeleportationState(TeleportationState.NotTeleporting);
    }
}

[Serializable]
public class TeleportationSetting
{
    public int id;
    public string logToStart;
    public Transform teleportPosition;
    [Range(0.5f, 2f)] public float stallStartTeleportationTime;
    [Range(0.5f, 2f)] public float startingTeleportationTime;
    [Range(0.5f, 2f)] public float endingTeleportationTime;
    [Range(0.5f, 2f)] public float stallEndTeleportationTime;
}
