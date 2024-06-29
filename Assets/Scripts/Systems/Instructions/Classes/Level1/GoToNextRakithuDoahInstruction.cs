using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextRakithuDoahInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        SecondObjectLearnedEnd.OnSecondObjectLearnedEnd += SecondObjectLearnedEnd_OnSecondObjectLearnedEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        SecondObjectLearnedEnd.OnSecondObjectLearnedEnd -= SecondObjectLearnedEnd_OnSecondObjectLearnedEnd;
    }

    private void SecondObjectLearnedEnd_OnSecondObjectLearnedEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}