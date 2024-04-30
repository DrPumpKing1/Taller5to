using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialectSymbolSource : MonoBehaviour
{
    [Header ("Symbol Source Settings")]
    [SerializeField] private DialectSymbolsSourceSO dialectSymbolSourceSO;
    
    public DialectSymbolsSourceSO DialectSymbolSourceSO { get { return dialectSymbolSourceSO; } }
}
