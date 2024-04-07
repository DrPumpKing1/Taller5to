using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IRequiresKnowledge 
{
    public List<DialectKnowledge> DialectKnowledgeRequirements { get; }

    public bool MeetsKnowledgeRequirements();
    public void KnowledgeRequirementsNotMet();

    public event EventHandler<OnKnowledgeRequirementsNotMetEventArgs> OnKnowledgeRequirementsNotMet;

    public class OnKnowledgeRequirementsNotMetEventArgs : EventArgs
    {
        public List<DialectKnowledge> dialectKnowledgeRequirements;
    }
}
