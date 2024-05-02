using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolCraftingSingleUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SymbolCraftingSO symbolCraftingSO;

    [Header("UI Components")]
    [SerializeField] private Image imageToTranslate;

    [Header("Settings")]
    [SerializeField] private bool isCrafted;

    public bool IsCrafted { get { return isCrafted; } }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        isCrafted = false;
    }

    public void SetSymbolCraftingSingleUI(SymbolCraftingSO symbolCraftingSO)
    {
        this.symbolCraftingSO = symbolCraftingSO;
        SetImageToTranslate(symbolCraftingSO.imageToTranslateSprite);
    }

    private void SetImageToTranslate(Sprite sprite) => imageToTranslate.sprite = sprite;
}
