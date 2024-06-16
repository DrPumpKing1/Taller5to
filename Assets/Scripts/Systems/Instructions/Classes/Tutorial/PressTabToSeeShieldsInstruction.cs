using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressTabToSeeShieldsInstruction : Instruction
{
    protected void OnEnable()
    {
        FirstShieldPieceCollectedEnd.OnFirstShieldPieceCollectedEnd += FirstShieldPieceCollectedEnd_OnFirstShieldPieceCollectedEnd;
        InventoryOpeningManager.OnInventoryOpen += InventoryOpeningManager_OnInventoryOpen;
    }

    protected void OnDisable()
    {
        FirstShieldPieceCollectedEnd.OnFirstShieldPieceCollectedEnd -= FirstShieldPieceCollectedEnd_OnFirstShieldPieceCollectedEnd;
        InventoryOpeningManager.OnInventoryOpen -= InventoryOpeningManager_OnInventoryOpen;
    }

    private void FirstShieldPieceCollectedEnd_OnFirstShieldPieceCollectedEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
    private void InventoryOpeningManager_OnInventoryOpen(object sender, System.EventArgs e)
    {
        hasBeenAcomplished = true;
    }
}
