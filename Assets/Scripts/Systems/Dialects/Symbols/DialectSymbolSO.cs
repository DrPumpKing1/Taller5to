using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewDialectSymbol", menuName = "ScriptableObjects/Dialects/DialectSymbol")]
public class DialectSymbolSO : ScriptableObject
{
    public int id;
    public Dialect dialect;
    public Sprite symbolImage;
    public Sprite meaningImage;
    public string _name;
    public string meaning;

    public List<DialectSymbolSO> originators;

    public bool IsPrimary() => originators.Count == 0;
}
