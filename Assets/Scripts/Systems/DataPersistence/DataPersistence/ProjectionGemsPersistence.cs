using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionGemsPersistence : MonoBehaviour, IDataPersistence<PlayerData>
{
    public void LoadData(PlayerData data)
    {
        ProjectionGemsManager projectionGemsManager = FindObjectOfType<ProjectionGemsManager>();

        if (data.totalProjectionGems == 0) return; //If its 0, it means PlayerData has been initialized as a new(), should avoid SetTotalProjectionGems to use the totalProjectionGems set in the inspector of ProjectionGemsManager

        projectionGemsManager.SetTotalProjectionGems(data.totalProjectionGems);
    }

    public void SaveData(ref PlayerData data)
    {
        ProjectionGemsManager projectionGemsManager = FindObjectOfType<ProjectionGemsManager>();

        data.totalProjectionGems = projectionGemsManager.TotalProjectionGems;
    }
}
