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

    public void AddSymbolToDictionary(DialectSymbolSO dialectSymbolSO)
    {
        if (symbolsDictionary.Contains(dialectSymbolSO)) return;

        symbolsDictionary.Add(dialectSymbolSO);
        OnDialectSymbolAddedToDictionary?.Invoke(this, new OnDialectSymbolAddedToDictionaryEventArgs { dialectSymbolSO = dialectSymbolSO });
        return;
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
