using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LearningManager : MonoBehaviour
{
    public static LearningManager Instance { get; private set; }

    [SerializeField] private List<ProjectableObjectSO> objectsLearned = new List<ProjectableObjectSO>();

    public static EventHandler<OnObjectLearnedEventArgs> OnObjectLearned;

    public class OnObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectLearned;
    }

    private void Awake()
    {
        SetSingleton();
    }
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one LearningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void LearnObject(ProjectableObjectSO projectableObjectToLearn)
    {
        objectsLearned.Add(projectableObjectToLearn);
        OnObjectLearned?.Invoke(this, new OnObjectLearnedEventArgs { projectableObjectLearned = projectableObjectToLearn });
    }
}
