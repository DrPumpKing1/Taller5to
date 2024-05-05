using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPlatformsUsedPersistenceManager : DataPersistenceManager<LearningPlatformsData>
{
    public static LearningPlatformsUsedPersistenceManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one LeraningPlatformsUsedPersistenceManager Instance");
        }

        Instance = this;
    }
}
