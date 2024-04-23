using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewDialectKnowledgeSourceSO", menuName = "ScriptableObjects/Dialects/DialectKnowledgeSource")]
public class DialectKnowledgeSourceSO : ScriptableObject
{
    public string _name;
    public List<DialectKnowledge> dialectKnowledgeLevelChanges = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);
    public DialectWritingSO dialectWritingSO;
}
