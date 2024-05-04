using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SymbolCraftingSingleUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SymbolCraftingSO symbolCraftingSO;

    [Header("UI Components")]
    [SerializeField] private Image imageToTranslate;

    [Header("Settings")]
    [SerializeField] private bool isCrafted;

    public bool IsCrafted { get { return isCrafted; } }

    public event EventHandler OnSymbolCrafted;

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        isCrafted = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) CraftSymbol(); //TestToCraftSymbol
    }
    
    #region SetUp Methods
    public void SetSymbolCraftingSingleUI(SymbolCraftingSO symbolCraftingSO)
    {
        this.symbolCraftingSO = symbolCraftingSO;
        SetImageToTranslate(symbolCraftingSO.imageToTranslateSprite);
    }

    private void SetImageToTranslate(Sprite sprite) => imageToTranslate.sprite = sprite;
    #endregion

    //Handle DrawingPointsLogic
    private void CraftSymbol()
    {
        isCrafted = true;
        OnSymbolCrafted?.Invoke(this, EventArgs.Empty);
    }
}
