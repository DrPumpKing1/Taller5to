using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataPersistenceManager : DataPersistenceManager<GameData>
{
    public static GameDataPersistenceManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one GameDataPersistenceManager Instance");
        }

        Instance = this;
    }
}
