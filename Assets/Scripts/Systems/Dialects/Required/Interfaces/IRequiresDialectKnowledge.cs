using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IRequiresDialectKnowledge 
{
    public List<DialectKnowledge> DialectKnowledgeRequirements { get; }

    public bool MeetsDialectKnowledgeRequirements();
    public void KnowledgeRequirementsNotMet();

    public event EventHandler<OnDialectKnowledgeRequirementsNotMetEventArgs> OnDialectKnowledgeRequirementsNotMet;

    public class OnDialectKnowledgeRequirementsNotMetEventArgs : EventArgs
    {
        public List<DialectKnowledge> dialectKnowledgeRequirements;
    }
}
