using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpeningPersistence : MonoBehaviour, IDataPersistence<UIData>
{
    public void LoadData(UIData data)
    {
        InventoryOpeningManager inventoryOpeningManager = FindObjectOfType<InventoryOpeningManager>();

        if (!data.canOpenInventory) return; //If its false, it means PlayerData has been initialized as a new()

        if (data.canOpenInventory) inventoryOpeningManager.SetCanOpenInventory(true);
        else inventoryOpeningManager.SetCanOpenInventory(false);
    }

    public void SaveData(ref UIData data)
    {
        InventoryOpeningManager inventoryOpeningManager = FindObjectOfType<InventoryOpeningManager>();

        data.canOpenInventory = inventoryOpeningManager.CanOpenInventory;
    }
}

