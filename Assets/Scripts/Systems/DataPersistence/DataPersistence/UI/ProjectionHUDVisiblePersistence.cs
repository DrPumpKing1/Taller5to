using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionHUDVisiblePersistence : MonoBehaviour, IDataPersistence<UIData>
{
    public void LoadData(UIData data)
    {
        ProjectionHUDVisibilityHandler projectionHUDVisibilityHandler = FindObjectOfType<ProjectionHUDVisibilityHandler>();

        if (!data.projectionHUDVisible) return; //If its false, it means PlayerData has been initialized as a new()

        if (data.projectionHUDVisible) projectionHUDVisibilityHandler.SetIsVisible(true);
        else projectionHUDVisibilityHandler.SetIsVisible(false);
    }

    public void SaveData(ref UIData data)
    {
        ProjectionHUDVisibilityHandler projectionHUDVisibilityHandler = FindObjectOfType<ProjectionHUDVisibilityHandler>();

        data.projectionHUDVisible = projectionHUDVisibilityHandler.IsVisible;
    }
}
