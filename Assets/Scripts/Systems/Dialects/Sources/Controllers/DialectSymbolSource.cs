using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialectSymbolSource : MonoBehaviour
{
    [Header ("Symbol Source Settings")]
    [SerializeField] private DialectSymbolSourceSO dialectSymbolSourceSO;

    [Header("Identifiers")]
    [SerializeField] private bool isCollected;

    public DialectSymbolSourceSO DialectSymbolSourceSO { get { return dialectSymbolSourceSO; } }
    public bool IsCollected => isCollected;

    public void SetIsCollected() => isCollected = true;
}
