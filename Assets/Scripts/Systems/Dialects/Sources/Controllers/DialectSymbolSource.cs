using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialectSymbolSource : MonoBehaviour
{
    [Header ("Symbol Source Settings")]
    [SerializeField] private DialectSymbolsSourceSO dialectSymbolSourceSO;

    [Header("Identifiers")]
    [SerializeField] private int id;
    [SerializeField] private bool isCollected;

    public DialectSymbolsSourceSO DialectSymbolSourceSO { get { return dialectSymbolSourceSO; } }
    public int ID => id;
    public bool IsCollected => isCollected;

    public void SetIsCollected() => isCollected = true;
}
