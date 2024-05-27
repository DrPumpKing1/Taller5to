using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SymbolSourcesManager : MonoBehaviour
{
    public static SymbolSourcesManager Instance { get; private set; }

    [Header("Symbols Sources Settings")]
    [SerializeField] private List<SymbolSourceSO> symbolSourcesCollected;
    [SerializeField] private List<SymbolSourceSO> completeSymbolSourcesPool;

    [Header("Debug")]
    [SerializeField] private bool debug;
    public List<SymbolSourceSO> SymbolSourcesCollected { get { return symbolSourcesCollected; } }
    public List<SymbolSourceSO> CompleteSymbolSourcesPool { get { return completeSymbolSourcesPool; } }

    public static event EventHandler<OnSymbolSourceCollectedEventArgs> OnSymbolSourceCollected;

    public class OnSymbolSourceCollectedEventArgs : EventArgs
    {
        public SymbolSourceSO collectedSource;
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
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one SymbolSourcesManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void CollectSource(SymbolSourceSO collectedSource)
    {
        AddSourceToInventory(collectedSource);
        OnSymbolSourceCollected?.Invoke(this, new OnSymbolSourceCollectedEventArgs { collectedSource = collectedSource });
    }

    private void AddSourceToInventory(SymbolSourceSO sourceToAdd)
    {
        if (symbolSourcesCollected.Contains(sourceToAdd))
        {
            if (debug) Debug.Log($"Symbols Dictionary already contains sourceToAdd with name: {sourceToAdd._name}");
            return;
        }

        symbolSourcesCollected.Add(sourceToAdd);
    }

    public void AddSourceToInventoryById(int id)
    {
        SymbolSourceSO sourceToAdd = GetSourceInCompletePoolById(id);

        if (!sourceToAdd)
        {
            if (debug) Debug.LogWarning("Addition will be ignored due to source not found");
            return;
        }

        if (CheckIfInventoryContainsSource(sourceToAdd))
        {
            if (debug) Debug.Log($"Sources Collected already contains symbolToAdd with id: {sourceToAdd.id}");
            return;
        }

        symbolSourcesCollected.Add(sourceToAdd);
    }

    public bool CheckIfInventoryContainsSource(SymbolSourceSO source) => symbolSourcesCollected.Contains(source);

    public bool CheckIfDictionaryContainsSymbolById(int id)
    {
        foreach (SymbolSourceSO source in symbolSourcesCollected)
        {
            if (source.id == id) return true;
        }

        return false;
    }

    public SymbolSourceSO GetSourceInCompletePoolById(int id)
    {
        foreach (SymbolSourceSO symbolSourceSO in completeSymbolSourcesPool)
        {
            if (symbolSourceSO.id == id) return symbolSourceSO;
        }

        if (debug) Debug.LogWarning($"Symbol Source with id {id} not found in completePool");
        return null;
    }

    public SymbolSourceSO GetSourceInDictionaryById(int id)
    {
        foreach (SymbolSourceSO symbolSourceSO in symbolSourcesCollected)
        {
            if (symbolSourceSO.id == id) return symbolSourceSO;
        }
        return null;
    }
}
