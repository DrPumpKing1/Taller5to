using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextXotarkDoahInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ThirdObjectLearnedEnd.OnThirdObjectLearnedEnd += ThirdObjectLearnedEnd_OnThirdObjectLearnedEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ThirdObjectLearnedEnd.OnThirdObjectLearnedEnd -= ThirdObjectLearnedEnd_OnThirdObjectLearnedEnd;
    }

    private void ThirdObjectLearnedEnd_OnThirdObjectLearnedEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}