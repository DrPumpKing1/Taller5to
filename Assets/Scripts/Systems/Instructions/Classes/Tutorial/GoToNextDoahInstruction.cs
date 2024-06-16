using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextDoahInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstObjectLearnedEnd.OnFirstObjectLearnedEnd += FirstObjectLearnedEnd_OnFirstObjectLearnedEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstObjectLearnedEnd.OnFirstObjectLearnedEnd -= FirstObjectLearnedEnd_OnFirstObjectLearnedEnd;
    }

    private void FirstObjectLearnedEnd_OnFirstObjectLearnedEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}