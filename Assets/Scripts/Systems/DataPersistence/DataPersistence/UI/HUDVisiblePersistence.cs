using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDVisiblePersistence : MonoBehaviour, IDataPersistence<UIData>
{
    public void LoadData(UIData data)
    {
        HUDVisibilityHandler HUDVisibilityHandler = FindObjectOfType<HUDVisibilityHandler>();

        if (!data.HUDVisible) return; //If its false, it means PlayerData has been initialized as a new()

        if (data.HUDVisible) HUDVisibilityHandler.SetIsVisible(true);
        else HUDVisibilityHandler.SetIsVisible(false);
    }

    public void SaveData(ref UIData data)
    {
        HUDVisibilityHandler HUDVisibilityHandler = FindObjectOfType<HUDVisibilityHandler>();

        data.HUDVisible = HUDVisibilityHandler.IsVisible;
    }
}
