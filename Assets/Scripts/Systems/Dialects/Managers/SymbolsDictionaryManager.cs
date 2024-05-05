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

    public List<DialectSymbolSO> SymbolsDictionary { get { return symbolsDictionary; } }
    public List<DialectSymbolSO> CompleteSymbolsPool { get { return completeSymbolsPool; } }

    public static event EventHandler OnDialectSymbolAddedToDictionary;

    public class OnDialectSymbolAddedToDictionaryEventArgs : EventArgs
    {
        public DialectSymbolSO dialectSymbolSO;
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

    public void AddSymbolToDictionary(DialectSymbolSO symbolToAdd, bool invokeEvents)
    {
        if (symbolsDictionary.Contains(symbolToAdd))
        {
            Debug.Log($"Symbols Dictionary already contains symbolToAdd withg name: {symbolToAdd._name}");
            return;
        }

        symbolsDictionary.Add(symbolToAdd);

        if (!invokeEvents) return;

        OnDialectSymbolAddedToDictionary?.Invoke(this, new OnDialectSymbolAddedToDictionaryEventArgs { dialectSymbolSO = symbolToAdd });
    }

    public void AddSymbolToDictionaryById(int id, bool invokeEvents)
    {
        DialectSymbolSO symbolToAdd = GetSymbolInCompletePoolById(id);

        if (!symbolToAdd)
        {
            Debug.LogWarning("Addition will be ignored due to symbol not found");
            return;
        }

        if (CheckIfDictionaryContainsSymbol(symbolToAdd))
        {
            Debug.Log($"Symbols Dictionary already contains symbolToAdd with id: {symbolToAdd.id}");
            return;
        }

        symbolsDictionary.Add(symbolToAdd);

        if (!invokeEvents) return;

        OnDialectSymbolAddedToDictionary?.Invoke(this, new OnDialectSymbolAddedToDictionaryEventArgs { dialectSymbolSO = symbolToAdd });
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

        Debug.LogWarning($"Dialect Symbol with id {id} not found in completePool");
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
