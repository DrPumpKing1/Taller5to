using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectableObjectsLearnedPersistence : MonoBehaviour, IDataPersistence<ProjectableObjectsLearnedData>
{
    public void LoadData(ProjectableObjectsLearnedData data)
    {
        ProjectableObjectsLearningManager projectableObjectsLearningManager = FindObjectOfType<ProjectableObjectsLearningManager>();

        foreach (KeyValuePair<int, bool> projectableObjectLearnedData in data.projectableObjectsLearned)
        {
            if (projectableObjectLearnedData.Value) projectableObjectsLearningManager.AddProjectableObjectToLearnedListById(projectableObjectLearnedData.Key);
        }
    }

    public void SaveData(ref ProjectableObjectsLearnedData data)
    {
        ProjectableObjectsLearningManager projectableObjectsLearningManager = FindObjectOfType<ProjectableObjectsLearningManager>();

        foreach (ProjectableObjectSO projectableObject in projectableObjectsLearningManager.CompleteProjectableObjectsPool) //Clear all data in data
        {
            if (data.projectableObjectsLearned.ContainsKey(projectableObject.id)) data.projectableObjectsLearned.Remove(projectableObject.id);
        }

        foreach (ProjectableObjectSO projectableObject in projectableObjectsLearningManager.CompleteProjectableObjectsPool)
        {
            bool collected = projectableObjectsLearningManager.CheckLearnedListContainsProjectableObject(projectableObject);

            data.projectableObjectsLearned.Add(projectableObject.id, collected);
        }
    }
}
