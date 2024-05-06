using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsDataPersistenceManager : DataPersistenceManager<ObjectsData>
{
    public static ObjectsDataPersistenceManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one ObjectsDataPersistenceManager Instance");
        }

        Instance = this;
    }

    //Subscribe to Checkpoint Manager To SaveGameData();
}

