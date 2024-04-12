using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectionGemsManager : MonoBehaviour
{
    public static ProjectionGemsManager Instance { get; private set; }

    [SerializeField] private int totalProyectionGems;
    [SerializeField] private int availableProyectionGems;

    public EventHandler<OnProjectionGemsEventArgs> OnProjectionGemsUsed;
    public EventHandler<OnProjectionGemsEventArgs> OnProjectionGemsRefunded;
    public EventHandler<OnProjectionGemsEventArgs> OnProjectionGemsIncreased;
    public EventHandler<OnProjectionGemsEventArgs> OnInsuficentProjectionGems;

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
            DontDestroyOnLoad(gameObject);
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
    }

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

    public void IncreaseProjectionGems(int quantity)
    {
        totalProyectionGems += quantity;
        OnProjectionGemsIncreased?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = quantity });
    }

    public void InsuficientProjectionGems(int tryToUseQuantity)
    {
        OnInsuficentProjectionGems?.Invoke(this, new OnProjectionGemsEventArgs { projectionGems = tryToUseQuantity });
    }
}
