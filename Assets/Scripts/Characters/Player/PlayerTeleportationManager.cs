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

    public enum TeleportationState { NotTeleporting, StartingTeleportation, EndingTelelportation };

    public static event EventHandler<OnTeleportationEventArgs> OnTeleportationStarted;
    public static event EventHandler<OnTeleportationEventArgs> OnTeleportationCompleted;
    public static event EventHandler<OnTeleportationEventArgs> OnTeleportationEnded;

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

    private void SetTeleportationState(TeleportationState state) => state = this.state;

    public void StartTeleportation(TeleportationSetting teleportationSetting)
    {
        if (state != TeleportationState.NotTeleporting) return;

        StopAllCoroutines();
        StartCoroutine(TeleportPlayerCoroutine(teleportationSetting));
    }

    private IEnumerator TeleportPlayerCoroutine(TeleportationSetting teleportationSetting)
    {
        SetTeleportationState(TeleportationState.StartingTeleportation);

        OnTeleportationStarted?.Invoke(this, new OnTeleportationEventArgs { teleportationSetting = teleportationSetting });

        yield return new WaitForSeconds(teleportationSetting.startingTeleportationTime);

        PlayerPositioningHandler.Instance.InstantPositionPlayer(teleportationSetting.teleportPosition.position);

        OnTeleportationCompleted?.Invoke(this, new OnTeleportationEventArgs { teleportationSetting = teleportationSetting });

        SetTeleportationState(TeleportationState.EndingTelelportation);

        yield return new WaitForSeconds(teleportationSetting.endingTeleportationTime);

        SetTeleportationState(TeleportationState.NotTeleporting);

        OnTeleportationEnded?.Invoke(this, new OnTeleportationEventArgs { teleportationSetting = teleportationSetting });
    }
}

[Serializable]
public class TeleportationSetting
{
    public int id;
    public string logToStart;
    public Transform teleportPosition;
    [Range(0.5f, 2f)] public float startingTeleportationTime;
    [Range(0.5f, 2f)] public float endingTeleportationTime;
}