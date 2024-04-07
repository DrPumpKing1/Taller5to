using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewKnowledgeSourceSO", menuName = "ScriptableObjects/Knowledge/KnowledgeSource")]
public class KnowledgeSourceSO : ScriptableObject
{
    public string _name;
    public List<DialectKnowledge> dialectKnowledgePercentageChanges = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);
}
