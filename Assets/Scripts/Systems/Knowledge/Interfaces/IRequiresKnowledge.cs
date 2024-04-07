using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRequiresKnowledge 
{
    public List<DialectKnowledge> DialectKnowledges { get; }

    public bool MeetsKnowledgeRequirements();
}
