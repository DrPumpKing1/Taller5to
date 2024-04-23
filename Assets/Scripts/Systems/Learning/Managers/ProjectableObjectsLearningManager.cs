using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectableObjectsLearningManager : MonoBehaviour
{
    public static ProjectableObjectsLearningManager Instance { get; private set; }

    [SerializeField] private List<ProjectableObjectSO> projectableObjectsLearned = new List<ProjectableObjectSO>();

    public static EventHandler<OnProjectableObjectLearnedEventArgs> OnProjectableObjectLearned;

    public List<ProjectableObjectSO> ProjectableObjectsLearned {  get { return projectableObjectsLearned; } }

    public class OnProjectableObjectLearnedEventArgs : EventArgs
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

    public void LearnProjectableObject(ProjectableObjectSO projectableObjectToLearn)
    {
        if (projectableObjectsLearned.Contains(projectableObjectToLearn)) return;

        projectableObjectsLearned.Add(projectableObjectToLearn);
        OnProjectableObjectLearned?.Invoke(this, new OnProjectableObjectLearnedEventArgs { projectableObjectLearned = projectableObjectToLearn });
    }
}
