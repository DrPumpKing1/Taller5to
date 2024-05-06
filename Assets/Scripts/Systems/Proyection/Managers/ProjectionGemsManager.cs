using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectionGemsManager : MonoBehaviour
{
    public static ProjectionGemsManager Instance { get; private set; }

    [Header ("Projection Gems Settings")]
    [SerializeField] private int totalProyectionGems;
    [SerializeField] private int availableProyectionGems;

    public int TotalProjectionGems => totalProyectionGems;
    public int AvailableProyectionGems => availableProyectionGems;

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
        availableProyectionGems = totalProyectionGems;
        OnProjectionGemsManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    public void SetTotalProjectionGems(int quantity) => totalProyectionGems = quantity;

    public bool CheckCanUseProjectionGems(int quantity) => availableProyectionGems >= quantity;

    public void UseProyectionGems(int quantity)
    {
        availableProyectionGems = availableProyectionGems - quantity < 0 ? 0 : availableProyectionGems - quantity;
        OnProjectionGemsUsed?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = quantity });
    }

    public void RefundProyectionGems(int quantity)
    {
        availableProyectionGems = availableProyectionGems + quantity > totalProyectionGems ? totalProyectionGems : availableProyectionGems + quantity;
        OnProjectionGemsRefunded?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = quantity });
    }

    public void IncreaseTotalProjectionGems(int quantity)
    {
        totalProyectionGems += quantity;
        availableProyectionGems += quantity;
        OnTotalProjectionGemsIncreased?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = quantity });
    }

    public void InsuficientProjectionGems(int tryToUseQuantity)
    {
        OnInsuficentProjectionGems?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = tryToUseQuantity });
    }
}
