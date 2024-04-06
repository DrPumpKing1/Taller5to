using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (fileName = "NewKnowledgeSettingsSO", menuName = "ScriptableObjects/Knowledge/KnowledgeSettings")]
public class KnowledgeSettingsSO : ScriptableObject
{
    public List<DialectKnowledge> dialectKnowledgeSettings = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);
}
