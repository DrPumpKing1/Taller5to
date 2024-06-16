using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackToDoahInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstShieldPieceCollectedEnd.OnFirstShieldPieceCollectedEnd += FirstShieldPieceCollectedEnd_OnFirstShieldPieceCollectedEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstShieldPieceCollectedEnd.OnFirstShieldPieceCollectedEnd -= FirstShieldPieceCollectedEnd_OnFirstShieldPieceCollectedEnd;
    }

    private void FirstShieldPieceCollectedEnd_OnFirstShieldPieceCollectedEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}
