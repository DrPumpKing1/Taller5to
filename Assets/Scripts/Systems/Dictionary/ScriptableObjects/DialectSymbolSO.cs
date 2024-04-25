using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewDialectWriting", menuName = "ScriptableObjects/Dialects/DialectSymbol")]
public class DialectSymbolSO : ScriptableObject
{
    public int id;
    public Dialect dialect;
    public Sprite symbolImage;
    public Sprite meaningImage;
    public string meaning;
    [Space]
    public List<DialectSymbolSO> originators;

    public bool IsPrimary() => originators.Count == 0;
}
