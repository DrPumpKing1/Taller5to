using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningPlatform : MonoBehaviour
{
    [Header("Leraning Platform Settings")]
    [SerializeField] private ProjectableObjectSO projectableObjectToLearn;

    public ProjectableObjectSO ProjectableObjectToLearn { get { return projectableObjectToLearn; } }

    public event EventHandler<OnObjectLearnedEventArgs> OnObjectLearned;

    public class OnObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO objectLearned;
    }

    public void LearnObject()
    {
        ProjectableObjectsLearningManager.Instance.LearnProjectableObject(projectableObjectToLearn);

        OnObjectLearned?.Invoke(this, new OnObjectLearnedEventArgs { objectLearned = projectableObjectToLearn });
    }
}
