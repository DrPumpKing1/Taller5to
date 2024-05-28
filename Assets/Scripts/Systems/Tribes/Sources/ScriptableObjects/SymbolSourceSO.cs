using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewSymbolSourceSO", menuName = "ScriptableObjects/Symbols/SymbolSource")]
public class SymbolSourceSO : ScriptableObject
{
    public int id;
    public string _name;
    public string description;
    public Sprite symbolSourceSprite;
    public List<SymbolSO> symbolSOs;
}
