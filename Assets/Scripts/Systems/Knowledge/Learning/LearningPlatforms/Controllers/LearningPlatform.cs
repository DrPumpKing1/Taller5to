using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatform : MonoBehaviour, IRequiresKnowledge
{
    [Header("Leraning Platform Settings")]
    [SerializeField] private ProjectableObjectSO projectableObjectToLearn;

    [Header("Requires Knowledge Settings")]
    [SerializeField] private List<DialectKnowledge> dialectKnowledgeRequirements = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);

    public List<DialectKnowledge> DialectKnowledgeRequirements => dialectKnowledgeRequirements;
    public ProjectableObjectSO ProjectableObjectToLearn { get { return projectableObjectToLearn; } }

    public event EventHandler<IRequiresKnowledge.OnKnowledgeRequirementsNotMetEventArgs> OnKnowledgeRequirementsNotMet;
    public event EventHandler<OnObjectLearnedEventArgs> OnObjectLearned;

    public class OnObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO objectLearned;
    }

    #region IRequiresKnowledge Methods
    public bool MeetsKnowledgeRequirements()
    {
        foreach (DialectKnowledge dialectKnowledge in KnowledgeManager.Instance.GetDialectKnowledges())
        {
            foreach (DialectKnowledge dialectKnowledgeRequirement in dialectKnowledgeRequirements)
            {
                if (dialectKnowledge.dialect == dialectKnowledgeRequirement.dialect)
                {
                    if (dialectKnowledge.level < dialectKnowledgeRequirement.level) return false;
                }
            }
        }

        return true;
    }
    public void KnowledgeRequirementsNotMet()
    {
        Debug.Log("Knowledge requirements not met");
        OnKnowledgeRequirementsNotMet?.Invoke(this, new IRequiresKnowledge.OnKnowledgeRequirementsNotMetEventArgs { dialectKnowledgeRequirements = dialectKnowledgeRequirements });
    }
    #endregion

    public void LearnObject()
    {
        LearningManager.Instance.LearnObject(projectableObjectToLearn);

        OnObjectLearned?.Invoke(this, new OnObjectLearnedEventArgs { objectLearned = projectableObjectToLearn });
    }
}
