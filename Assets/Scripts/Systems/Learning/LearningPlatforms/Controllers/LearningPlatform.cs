using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatform : MonoBehaviour, IRequiresDialectKnowledge
{
    [Header("Leraning Platform Settings")]
    [SerializeField] private ProjectableObjectSO projectableObjectToLearn;

    [Header("Requires Knowledge Settings")]
    [SerializeField] private List<DialectKnowledge> dialectKnowledgeRequirements = new List<DialectKnowledge>(Enum.GetValues(typeof(Dialect)).Length);

    public List<DialectKnowledge> DialectKnowledgeRequirements => dialectKnowledgeRequirements;
    public ProjectableObjectSO ProjectableObjectToLearn { get { return projectableObjectToLearn; } }

    public event EventHandler<IRequiresDialectKnowledge.OnDialectKnowledgeRequirementsNotMetEventArgs> OnDialectKnowledgeRequirementsNotMet;
    public event EventHandler<OnObjectLearnedEventArgs> OnObjectLearned;

    public class OnObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO objectLearned;
    }

    #region IRequiresKnowledge Methods
    public bool MeetsDialectKnowledgeRequirements()
    {
        foreach (DialectKnowledge dialectKnowledge in DialectManager.Instance.DialectKnowledges)
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
        OnDialectKnowledgeRequirementsNotMet?.Invoke(this, new IRequiresDialectKnowledge.OnDialectKnowledgeRequirementsNotMetEventArgs { dialectKnowledgeRequirements = dialectKnowledgeRequirements });
    }
    #endregion

    public void LearnObject()
    {
        ProjectableObjectsLearningManager.Instance.LearnProjectableObject(projectableObjectToLearn);

        OnObjectLearned?.Invoke(this, new OnObjectLearnedEventArgs { objectLearned = projectableObjectToLearn });
    }
}
