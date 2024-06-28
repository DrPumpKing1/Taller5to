using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectableObjectsLearningManager : MonoBehaviour
{
    public static ProjectableObjectsLearningManager Instance { get; private set; }

    [Header("Projectable Objects Learned Settings")]
    [SerializeField] private List<ProjectableObjectSO> projectableObjectsLearned = new List<ProjectableObjectSO>();
    [SerializeField] private List<ProjectableObjectSO> completeProjectableObjectsPool = new List<ProjectableObjectSO>();

    [Header("Debug")]
    [SerializeField] private bool debug;

    public static EventHandler<OnProjectableObjectLearnedEventArgs> OnProjectableObjectLearned;

    public List<ProjectableObjectSO> ProjectableObjectsLearned => projectableObjectsLearned;
    public List<ProjectableObjectSO> CompleteProjectableObjectsPool => completeProjectableObjectsPool;

    public class OnProjectableObjectLearnedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectLearned;
        public LearningPlatform learningPlatform;
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
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one ProjectionLearningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void LearnProjectableObject(ProjectableObjectSO projectableObjectToLearn, LearningPlatform learningPlatform)
    {
        if (CheckLearnedListContainsProjectableObject(projectableObjectToLearn))
        {
            if (debug) Debug.Log($"ProjectableObjectsLearned list already contains projectableObjectToLearn with name: {projectableObjectToLearn.objectName}");
            return;
        }

        AddProjectableObjectToLearnedList(projectableObjectToLearn);
        OnProjectableObjectLearned?.Invoke(this, new OnProjectableObjectLearnedEventArgs { projectableObjectLearned = projectableObjectToLearn, learningPlatform = learningPlatform });
    }

    public void AddProjectableObjectToLearnedList(ProjectableObjectSO objectToAdd)
    {
        if (CheckLearnedListContainsProjectableObject(objectToAdd))
        {
            if(debug) Debug.Log($"ProjectableObjectsLearned list already contains objectToAdd with name: {objectToAdd.objectName}");
            return;
        }

        projectableObjectsLearned.Add(objectToAdd);
    }

    public void AddProjectableObjectToLearnedListById(int id)
    {
        ProjectableObjectSO projectableObjectToAdd = GetProjectableObjectInCompletePoolById(id);

        if (!projectableObjectToAdd)
        {
            if (debug) Debug.LogWarning("Addition will be ignored due to projectable object not found");
            return;
        }

        if (CheckLearnedListContainsProjectableObject(projectableObjectToAdd))
        {
            if (debug) Debug.Log($"Projectable Objects Learned List already contains objectToAdd with id: {projectableObjectToAdd.id}");
            return;
        }

        projectableObjectsLearned.Add(projectableObjectToAdd);
    }

    public bool CheckLearnedListContainsProjectableObject(ProjectableObjectSO projectableObject) => projectableObjectsLearned.Contains(projectableObject);

    public bool CheckLearnedListContainsProjectableObjectById(int id)
    {
        foreach (ProjectableObjectSO projectableObject in projectableObjectsLearned)
        {
            if (projectableObject.id == id) return true;
        }

        return false;
    }

    public ProjectableObjectSO GetProjectableObjectInCompletePoolById(int id)
    {
        foreach (ProjectableObjectSO projectableObjectSO in completeProjectableObjectsPool)
        {
            if (projectableObjectSO.id == id) return projectableObjectSO;
        }

        if (debug) Debug.LogWarning($"Projectable Object with id {id} not found in completePool");
        return null;
    }

    public ProjectableObjectSO GetProjectableObjectInLearnedListById(int id)
    {
        foreach (ProjectableObjectSO projectableObjectSO in completeProjectableObjectsPool)
        {
            if (projectableObjectSO.id == id) return projectableObjectSO;
        }

        return null;
    }
}
