using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionGemsPersistenceUnified : MonoBehaviour, IDataPersistence<GameData>
{
    public void LoadData(GameData data)
    {
        ProjectionGemsManager projectionGemsManager = FindObjectOfType<ProjectionGemsManager>();

        if (data.totalProjectionGems == 0) return; //If its 0, it means GameData has been initialized as a new()

        projectionGemsManager.SetTotalProjectionGems(data.totalProjectionGems);
    }

    public void SaveData(ref GameData data)
    {
        ProjectionGemsManager projectionGemsManager = FindObjectOfType<ProjectionGemsManager>();

        data.totalProjectionGems = projectionGemsManager.TotalProjectionGems;
    }
}

