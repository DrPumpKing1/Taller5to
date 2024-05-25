using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInstruction : Instruction
{
    [Header("Components")]
    [SerializeField] private SymbolCraftingUI symbolCraftingUI;

    [Header("Settings")]
    [SerializeField] private bool triggerOnAnySymbolCraftingUIOpen;

    private void OnEnable()
    {
        SymbolCraftingUI.OnAnySymbolCraftingUIOpen += SymbolCraftingUI_OnAnySymbolCraftingUIOpen;
    }

    private void OnDisable()
    {
        SymbolCraftingUI.OnAnySymbolCraftingUIOpen += SymbolCraftingUI_OnAnySymbolCraftingUIOpen;
    }

    #region SymbolCraftingUI Subscriptions
    private void SymbolCraftingUI_OnAnySymbolCraftingUIOpen(object sender, SymbolCraftingUI.OnAnySymbolCraftingUIEventArgs e)
    {
        if (!triggerOnAnySymbolCraftingUIOpen && e.symbolCraftingUI != symbolCraftingUI) return;
        HandleInstructionTrigger();
    }
    #endregion
}
