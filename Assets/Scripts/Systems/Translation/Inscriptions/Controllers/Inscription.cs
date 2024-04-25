using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscription : MonoBehaviour
{
    [Header("Inscription Settings")]
    [SerializeField] private SymbolCraftingSO symbolCraftingSO;

    public SymbolCraftingSO SymbolCraftingSO { get { return symbolCraftingSO; } }
}
