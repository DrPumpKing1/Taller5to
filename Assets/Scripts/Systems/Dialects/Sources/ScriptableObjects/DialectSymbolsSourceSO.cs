using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewDialectSymbolSourceSO", menuName = "ScriptableObjects/Dialects/DialectSymbolSource")]
public class DialectSymbolsSourceSO : ScriptableObject
{
    public string _name;
    public List<DialectSymbolSO> dialectSymbolSOs;
}
