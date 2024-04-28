using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager Instance { get; private set; }

    [Header("Dialect Writings Settings")]
    [SerializeField] private List<DialectDictionary> dictionary;

    public List<DialectDictionary> Dictionary { get { return dictionary; } }

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
            Debug.LogWarning("There is more than one InventoryManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void AddSymbolToDictionary(DialectSymbolSO dialectSymbolSO)
    {
        foreach (DialectDictionary dialectDictionary in dictionary)
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
        foreach (DialectDictionary dialectDictionary in dictionary)
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
