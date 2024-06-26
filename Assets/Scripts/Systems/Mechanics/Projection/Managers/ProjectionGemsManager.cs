using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectionGemsManager : MonoBehaviour
{
    public static ProjectionGemsManager Instance { get; private set; }

    [Header ("Projection Gems Settings")]
    [SerializeField] private int totalProjectionGems;
    [SerializeField] private int availableProjectionGems;

    public int TotalProjectionGems => totalProjectionGems;
    public int AvailableProjectionGems => availableProjectionGems;

    public static EventHandler<OnProjectionGemsEventArgs> OnProjectionGemsUsed;
    public static EventHandler<OnProjectionGemsEventArgs> OnProjectionGemsRefunded;
    public static EventHandler<OnProjectionGemsEventArgs> OnTotalProjectionGemsIncreased;
    public static EventHandler<OnProjectionGemsEventArgs> OnInsuficentProjectionGems;

    public static EventHandler OnProjectionGemsManagerInitialized;

    public class OnProjectionGemsEventArgs: EventArgs
    {
        public int projectionGems;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
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
            Debug.LogWarning("There is more than one ProjectionGemsManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        availableProjectionGems = totalProjectionGems;
        OnProjectionGemsManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public void SetTotalProjectionGems(int quantity) => totalProjectionGems = quantity;

    public bool CheckCanUseProjectionGems(int quantity) => availableProjectionGems >= quantity;

    public void UseProjectionGems(int quantity)
    {
        availableProjectionGems = availableProjectionGems - quantity < 0 ? 0 : availableProjectionGems - quantity;
        OnProjectionGemsUsed?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = quantity });
    }

    public void RefundProjectionGems(int quantity)
    {
        availableProjectionGems = availableProjectionGems + quantity > totalProjectionGems ? totalProjectionGems : availableProjectionGems + quantity;
        OnProjectionGemsRefunded?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = quantity });
    }

    public void RefundAllProjectionGems()
    {
        int refundedProjectionGems = totalProjectionGems - availableProjectionGems;
        availableProjectionGems = totalProjectionGems;
        OnProjectionGemsRefunded?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = refundedProjectionGems });
    }

    public void IncreaseTotalProjectionGems(int quantity)
    {
        totalProjectionGems += quantity;
        availableProjectionGems += quantity;
        OnTotalProjectionGemsIncreased?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = quantity });
    }

    public void InsuficientProjectionGems(int tryToUseQuantity)
    {
        OnInsuficentProjectionGems?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = tryToUseQuantity });
    }

    public bool HasFullProjectionGems() => availableProjectionGems >= totalProjectionGems;
}
