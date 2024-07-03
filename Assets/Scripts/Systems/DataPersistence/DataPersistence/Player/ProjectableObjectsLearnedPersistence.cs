using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectableObjectsLearnedPersistence : MonoBehaviour, IDataPersistence<PlayerData>
{
    public void LoadData(PlayerData data)
    {
        List<int> projectableObjectsLearnedIDs = new List<int>();
        
        foreach (KeyValuePair<int, bool> projectableObjectLearnedData in data.projectableObjectsLearned)
        {
            if (projectableObjectLearnedData.Value) projectableObjectsLearnedIDs.Add(projectableObjectLearnedData.Key);
        }

        projectableObjectsLearnedIDs.Sort();
        ProjectableObjectsLearningManager projectableObjectsLearningManager = FindObjectOfType<ProjectableObjectsLearningManager>();

        foreach (int projectableObjectLearnedID in projectableObjectsLearnedIDs)
        {
            projectableObjectsLearningManager.AddProjectableObjectToLearnedListById(projectableObjectLearnedID);
        }
    }

    public void SaveData(ref PlayerData data)
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
