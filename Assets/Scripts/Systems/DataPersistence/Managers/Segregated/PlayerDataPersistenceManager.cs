using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataPersistenceManager : DataPersistenceManager<PlayerData>
{
    public static PlayerDataPersistenceManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one PlayerDataPersistenceManager Instance");
        }

        Instance = this;
    }

    //Subscribe to Checkpoint Manager To SaveGameData();
}
