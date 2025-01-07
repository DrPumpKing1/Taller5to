using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggsManager : MonoBehaviour
{
    public EasterEggsManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private int seed;

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static event EventHandler<OnSeedEventArgs> OnSeedSet;
    public static event EventHandler<OnSeedEventArgs> OnSeedGenerated;

    private const int MIN_SEED_NUMBER = 1;
    private const int MAX_SEED_NUMBER = 1000;

    public class OnSeedEventArgs : EventArgs
    {
        public int seed;
    }

    public int Seed => seed;


    private void Awake()
    {
        SetSingleton();
        //Seed is loaded or initialized from EasterEggsDataPersistenceManager. If initialized, it is 0
    }

    private void Start()
    {
        GenerateSeed();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one EasterEggsManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void SetSeed(int seed)
    {
        if (!SeedGenerated(seed)) return; //Check if the seed value is from a generated seed

        this.seed = seed;
        OnSeedSet?.Invoke(this, new OnSeedEventArgs { seed = seed });

        if (debug) Debug.Log($"Seed Set: {seed}");
    }

    private void GenerateSeed()
    {
        if (SeedGenerated(this.seed)) return; //Check if this manager's seed has been generated

        seed = UnityEngine.Random.Range(MIN_SEED_NUMBER,MAX_SEED_NUMBER+1);
        OnSeedGenerated?.Invoke(this, new OnSeedEventArgs {seed = seed});

        if(debug) Debug.Log($"Seed Generated: {seed}");
    }

    private bool SeedGenerated(int seedToCheck) => seedToCheck > 0; //If seedToCheck is 0, seed has not been generated
}
