using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IRequiresSymbolCrafting
{
    public void OpenSymbolCraftingUI();
    public void CraftSymbol();
    public bool SymbolCrafted { get; }
}
