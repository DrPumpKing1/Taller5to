using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DematerializationAllSFXManager : CustomSFXManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ProjectionResetObject.OnAnyStartProjectionResetObjectUse += ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse += ProjectionResetObject_OnAnyEndProjectionResetObjectUse;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ProjectionResetObject.OnAnyStartProjectionResetObjectUse -= ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse -= ProjectionResetObject_OnAnyEndProjectionResetObjectUse;
    }

    private void ProjectionResetObject_OnAnyStartProjectionResetObjectUse(object sender, EventArgs e)
    {
        ReplaceAudioClip(SFXPoolSO.startDematerializationAll);
    }
    private void ProjectionResetObject_OnAnyEndProjectionResetObjectUse(object sender, EventArgs e)
    {
        StopAudioSource();
    }
}
