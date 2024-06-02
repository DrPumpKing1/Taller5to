using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIData
{
    public bool HUDVisible;
    public bool canOpenInventory;

    public UIData()
    {
        HUDVisible = false;
        canOpenInventory = false;
    }
}
