using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SymbolsDictionaryManager : MonoBehaviour
{
    public static SymbolsDictionaryManager Instance { get; private set; }

    [Header("Dialect Symbols Settings")]
    [SerializeField] private List<DialectDictionary> dialectDictionaries;

    public List<DialectDictionary> DialectDictionaries { get { return dialectDictionaries; } }

    public static event EventHandler OnDialectSymbolAddedToDictionary;

    public class OnDialectSymbolAddedToDictionaryEventArgs : EventArgs
    {
        public DialectDictionary dialectDictionary;
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
        foreach (DialectDictionary dialectDictionary in dialectDictionaries)
        {
            if (dialectDictionary.dialect == dialectSymbolSO.dialect)
            {
                if (dialectDictionary.dialectSymbolsSOs.Contains(dialectSymbolSO)) return;

                dialectDictionary.dialectSymbolsSOs.Add(dialectSymbolSO);
                OnDialectSymbolAddedToDictionary?.Invoke(this, new OnDialectSymbolAddedToDictionaryEventArgs { dialectDictionary = dialectDictionary, dialectSymbolSO = dialectSymbolSO });
                return;
            }
        }
    }

    public DialectDictionary GetDialectDictionaryByDialect(Dialect dialect)
    {
        foreach (DialectDictionary dialectDictionary in dialectDictionaries)
        {
            if (dialectDictionary.dialect == dialect)
            {
                return dialectDictionary;
            }
        }

        Debug.LogWarning($"The dialect {dialect} does not match any dictionary on DicionaryManager");
        return null;
    }
}
