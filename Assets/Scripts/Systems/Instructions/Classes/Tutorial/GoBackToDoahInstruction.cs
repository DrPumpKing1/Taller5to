using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToDoahInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        InventoryOpeningManager.OnInventoryClose += InventoryOpeningManager_OnInventoryClose;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InventoryOpeningManager.OnInventoryClose -= InventoryOpeningManager_OnInventoryClose;
    }

    private void InventoryOpeningManager_OnInventoryClose(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}
