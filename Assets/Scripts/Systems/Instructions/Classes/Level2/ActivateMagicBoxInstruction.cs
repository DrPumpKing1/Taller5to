using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMagicBoxInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstMagicBoxProjectionEnd.OnFirstMagicBoxProjectionEnd += FirstMagicBoxProjectionEnd_OnFirstMagicBoxProjectionEnd; ;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstMagicBoxProjectionEnd.OnFirstMagicBoxProjectionEnd += FirstMagicBoxProjectionEnd_OnFirstMagicBoxProjectionEnd; ;
    }

    private void FirstMagicBoxProjectionEnd_OnFirstMagicBoxProjectionEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}