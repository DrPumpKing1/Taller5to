using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSenderInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstSenderProjectionEnd.OnFirstSenderProjectionEnd += FirstSenderProjectionEnd_OnFirstSenderProjectionEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstSenderProjectionEnd.OnFirstSenderProjectionEnd -= FirstSenderProjectionEnd_OnFirstSenderProjectionEnd;
    }

    private void FirstSenderProjectionEnd_OnFirstSenderProjectionEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}