using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SymbolsDictionaryManager : MonoBehaviour
{
    public static SymbolsDictionaryManager Instance { get; private set; }

    [Header("Symbols Dictionary Settings")]
    [SerializeField] private List<DialectSymbolSO> symbolsDictionary;
    [SerializeField] private List<DialectSymbolSO> completeSymbolsPool;

    [Header("Debug")]
    [SerializeField] private bool debug;
    public List<DialectSymbolSO> SymbolsDictionary { get { return symbolsDictionary; } }
    public List<DialectSymbolSO> CompleteSymbolsPool { get { return completeSymbolsPool; } }

    public static event EventHandler OnDialectSymbolCollected;

    public class OnDialectSymbolCollectedEventArgs : EventArgs
    {
        public DialectSymbolSO collectedSymbol;
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one SymbolsDictionaryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void CollectSymbol(DialectSymbolSO collectedSymbol)
    {
        AddSymbolToDictionary(collectedSymbol);
        OnDialectSymbolCollected?.Invoke(this, new OnDialectSymbolCollectedEventArgs { collectedSymbol = collectedSymbol });
    }

    public void AddSymbolToDictionary(DialectSymbolSO symbolToAdd)
    {
        if (symbolsDictionary.Contains(symbolToAdd))
        {
            if (debug) Debug.Log($"Symbols Dictionary already contains symbolToAdd with name: {symbolToAdd._name}");
            return;
        }

        symbolsDictionary.Add(symbolToAdd); 
    }

    public void AddSymbolToDictionaryById(int id)
    {
        DialectSymbolSO symbolToAdd = GetSymbolInCompletePoolById(id);

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

        symbolsDictionary.Add(symbolToAdd);
    }

    public bool CheckIfDictionaryContainsSymbol(DialectSymbolSO symbol) => symbolsDictionary.Contains(symbol);

    public bool CheckIfDictionaryContainsSymbolById(int id)
    {
        foreach(DialectSymbolSO symbol in symbolsDictionary)
        {
            if (symbol.id == id) return true;
        }

        return false;
    }

    public DialectSymbolSO GetSymbolInCompletePoolById(int id)
    {
        foreach (DialectSymbolSO dialectSymbolSO in completeSymbolsPool)
        {
            if (dialectSymbolSO.id == id) return dialectSymbolSO;
        }

        if (debug) Debug.LogWarning($"Dialect Symbol with id {id} not found in completePool");
        return null;
    }

    public DialectSymbolSO GetSymbolInDictionaryById(int id)
    {
        foreach (DialectSymbolSO dialectSymbolSO in symbolsDictionary)
        {
            if (dialectSymbolSO.id == id) return dialectSymbolSO;
        }
        return null;
    }

}
