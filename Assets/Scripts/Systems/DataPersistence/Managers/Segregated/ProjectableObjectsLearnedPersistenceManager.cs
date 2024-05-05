using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectableObjectsLearnedPersistenceManager : DataPersistenceManager<ProjectableObjectsLearnedData>
{
    public static ProjectableObjectsLearnedPersistenceManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one ProjectableObjectsLearnedPersistenceManager Instance");
        }

        Instance = this;
    }
}
