using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewKnowledgeSourceSO", menuName = "ScriptableObjects/Knowledge/KnowledgeSource/KnowledgeSource")]
public class KnowledgeSourceSO : ScriptableObject
{
    public string _name;
    public List<DialectKnowledge> dialectKnowledgeLevelChanges = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);
}
