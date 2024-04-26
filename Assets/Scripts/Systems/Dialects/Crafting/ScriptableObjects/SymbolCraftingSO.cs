using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialectSymbolCrafting", menuName = "ScriptableObjects/Dialects/DialectSymbolCrafting")]
public class SymbolCraftingSO : ScriptableObject
{
    public DialectSymbolSO symbolToCraft;
    public Sprite imageToTranslateSprite;
    public List<Vector2> symbolCraftingPoints;
}
