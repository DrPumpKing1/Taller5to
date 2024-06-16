using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickShieldInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstInscriptionPoweringEnd.OnFirstInscriptionPoweringEnd += FirstInscriptionPoweringEnd_OnFirstInscriptionPoweringEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstInscriptionPoweringEnd.OnFirstInscriptionPoweringEnd -= FirstInscriptionPoweringEnd_OnFirstInscriptionPoweringEnd;
    }

    private void FirstInscriptionPoweringEnd_OnFirstInscriptionPoweringEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}
