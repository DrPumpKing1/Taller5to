using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Dialect Writings Settings")]
    [SerializeField] private List<DialectWritingSO> dialectWritings;

    public List<DialectWritingSO> DialectWritingSOs { get { return DialectWritingSOs; } }

    public event EventHandler<OnDialectWritingAddedToInventoryEventArgs> OnDialectWritingAddedToInventory;

    public class OnDialectWritingAddedToInventoryEventArgs
    {
        public DialectWritingSO dialectWritingSO;
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

    public void AddDialectWritingToInventory(DialectWritingSO dialectWritingSO)
    {
        dialectWritings.Add(dialectWritingSO);
        OnDialectWritingAddedToInventory?.Invoke(this, new OnDialectWritingAddedToInventoryEventArgs { dialectWritingSO = dialectWritingSO });
    }
}
