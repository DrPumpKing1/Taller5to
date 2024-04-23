using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (fileName = "NewDialectKnowledgeSettingsSO", menuName = "ScriptableObjects/Dialects/DialectKnowledgeSettings")]
public class DialectKnowledgeSettingsSO : ScriptableObject
{
    public List<DialectKnowledge> dialectKnowledgeSettings = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);
}
