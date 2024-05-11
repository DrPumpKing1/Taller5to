using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewDialectSymbolSourceSO", menuName = "ScriptableObjects/Dialects/DialectSymbolSource")]
public class DialectSymbolSourceSO : ScriptableObject
{
    public int id;
    public string _name;
    public string description;
    public Sprite symbolSourceSprite;
    public List<DialectSymbolSO> dialectSymbolSOs;
}
