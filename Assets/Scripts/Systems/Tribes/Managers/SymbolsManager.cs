using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SymbolsManager : MonoBehaviour
{
    public static SymbolsManager Instance { get; private set; }

    [Header("Symbols Dictionary Settings")]
    [SerializeField] private List<SymbolSO> symbolsCollected;
    [SerializeField] private List<SymbolSO> completeSymbolsPool;

    [Header("Debug")]
    [SerializeField] private bool debug;
    public List<SymbolSO> SymbolsCollected { get { return symbolsCollected; } }
    public List<SymbolSO> CompleteSymbolsPool { get { return completeSymbolsPool; } }

    public static event EventHandler<OnSymbolCollectedEventArgs> OnSymbolCollected;

    public class OnSymbolCollectedEventArgs : EventArgs
    {
        public SymbolSO collectedSymbol;
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
            Debug.LogWarning("There is more than one SymbolsDictionaryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void CollectSymbol(SymbolSO collectedSymbol)
    {
        AddSymbolToDictionary(collectedSymbol);
        OnSymbolCollected?.Invoke(this, new OnSymbolCollectedEventArgs { collectedSymbol = collectedSymbol });
    }

    private void AddSymbolToDictionary(SymbolSO symbolToAdd)
    {
        if (symbolsCollected.Contains(symbolToAdd))
        {
            if (debug) Debug.Log($"Symbols Dictionary already contains symbolToAdd with name: {symbolToAdd._name}");
            return;
        }

        symbolsCollected.Add(symbolToAdd); 
    }

    public void AddSymbolToDictionaryById(int id)
    {
        SymbolSO symbolToAdd = GetSymbolInCompletePoolById(id);

        if (!symbolToAdd)
        {
            if (debug) Debug.LogWarning("Addition will be ignored due to symbol not found");
            return;
        }

        if (CheckIfDictionaryContainsSymbol(symbolToAdd))
        {
            if (debug) Debug.Log($"Symbols Dictionary already contains symbolToAdd with id: {symbolToAdd.id}");
            return;
        }

        symbolsCollected.Add(symbolToAdd);
    }

    public bool CheckIfDictionaryContainsSymbol(SymbolSO symbol) => symbolsCollected.Contains(symbol);

    public bool CheckIfDictionaryContainsSymbolById(int id)
    {
        foreach(SymbolSO symbol in symbolsCollected)
        {
            if (symbol.id == id) return true;
        }

        return false;
    }

    public SymbolSO GetSymbolInCompletePoolById(int id)
    {
        foreach (SymbolSO symbolSO in completeSymbolsPool)
        {
            if (symbolSO.id == id) return symbolSO;
        }

        if (debug) Debug.LogWarning($"Symbol with id {id} not found in completePool");
        return null;
    }

    public SymbolSO GetSymbolInDictionaryById(int id)
    {
        foreach (SymbolSO symbolSO in symbolsCollected)
        {
            if (symbolSO.id == id) return symbolSO;
        }
        return null;
    }

}
