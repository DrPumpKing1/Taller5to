using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialectKnowledgeSource : MonoBehaviour
{
    [Header ("Knowledge Source Settings")]
    [SerializeField] private DialectKnowledgeSourceSO dialectKnowledgeSourceSO;
    
    public DialectKnowledgeSourceSO DialectKnowledgeSourceSO { get { return dialectKnowledgeSourceSO; } }
}
