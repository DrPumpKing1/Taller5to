using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIData
{
    public bool projectionHUDVisible;
    public bool canOpenInventory;
    public bool canOpenJournal;

    public UIData()
    {
        projectionHUDVisible = false;

        canOpenInventory = false;
        canOpenJournal = false;
    }
}
