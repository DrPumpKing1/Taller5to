using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SymbolSourcesManager : MonoBehaviour
{
    public static SymbolSourcesManager Instance { get; private set; }

    [Header("Symbols Sources Settings")]
    [SerializeField] private List<DialectSymbolSourceSO> symbolSourcesCollected;
    [SerializeField] private List<DialectSymbolSourceSO> completeSymbolSourcesPool;

    [Header("Debug")]
    [SerializeField] private bool debug;
    public List<DialectSymbolSourceSO> SymbolSourcesCollected { get { return symbolSourcesCollected; } }
    public List<DialectSymbolSourceSO> CompleteSymbolSourcesPool { get { return completeSymbolSourcesPool; } }

    public static event EventHandler<OnDialectSymbolSourceCollectedEventArgs> OnDialectSymbolSourceCollected;

    public class OnDialectSymbolSourceCollectedEventArgs : EventArgs
    {
        public DialectSymbolSourceSO collectedSource;
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

    public void CollectSource(DialectSymbolSourceSO collectedSource)
    {
        AddSourceToInventory(collectedSource);
        OnDialectSymbolSourceCollected?.Invoke(this, new OnDialectSymbolSourceCollectedEventArgs { collectedSource = collectedSource });
    }

    private void AddSourceToInventory(DialectSymbolSourceSO sourceToAdd)
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
        DialectSymbolSourceSO sourceToAdd = GetSourceInCompletePoolById(id);

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

    public bool CheckIfInventoryContainsSource(DialectSymbolSourceSO source) => symbolSourcesCollected.Contains(source);

    public bool CheckIfDictionaryContainsSymbolById(int id)
    {
        foreach (DialectSymbolSourceSO source in symbolSourcesCollected)
        {
            if (source.id == id) return true;
        }

        return false;
    }

    public DialectSymbolSourceSO GetSourceInCompletePoolById(int id)
    {
        foreach (DialectSymbolSourceSO dialectSymbolSourceSO in completeSymbolSourcesPool)
        {
            if (dialectSymbolSourceSO.id == id) return dialectSymbolSourceSO;
        }

        if (debug) Debug.LogWarning($"Dialect Symbol Source with id {id} not found in completePool");
        return null;
    }

    public DialectSymbolSourceSO GetSourceInDictionaryById(int id)
    {
        foreach (DialectSymbolSourceSO dialectSymbolSourceSO in symbolSourcesCollected)
        {
            if (dialectSymbolSourceSO.id == id) return dialectSymbolSourceSO;
        }
        return null;
    }
}
