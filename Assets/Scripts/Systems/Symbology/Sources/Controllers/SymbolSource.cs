using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSource : MonoBehaviour
{
    [Header ("Symbol Source Settings")]
    [SerializeField] private SymbolSourceSO symbolSourceSO;

    [Header("Identifiers")]
    [SerializeField] private bool isCollected;

    public SymbolSourceSO SymbolSourceSO => symbolSourceSO;
    public bool IsCollected => isCollected;

    public void SetIsCollected() => isCollected = true;
}
