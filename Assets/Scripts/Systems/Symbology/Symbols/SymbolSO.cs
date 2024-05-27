using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewSymbolSO", menuName = "ScriptableObjects/Symbols/Symbol")]
public class SymbolSO : ScriptableObject
{
    public int id;
    public Dialect dialect;
    public Sprite symbolImage;
    public Sprite meaningImage;
    public string _name;
    public string meaning;

    public List<SymbolSO> originators;

    public bool IsPrimary() => originators.Count == 0;
}
