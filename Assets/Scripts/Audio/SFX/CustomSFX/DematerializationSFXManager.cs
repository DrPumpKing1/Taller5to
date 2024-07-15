using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DematerializationSFXManager : CustomSFXManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ProjectableObjectDematerialization.OnStartDematerialization += ProjectableObjectDematerialization_OnStartDematerialization;
        ProjectableObjectDematerialization.OnEndDematerialization += ProjectableObjectDematerialization_OnEndDematerialization;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ProjectableObjectDematerialization.OnStartDematerialization -= ProjectableObjectDematerialization_OnStartDematerialization;
        ProjectableObjectDematerialization.OnEndDematerialization -= ProjectableObjectDematerialization_OnEndDematerialization;
    }

    private void ProjectableObjectDematerialization_OnStartDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        ReplaceAudioClip(SFXPoolSO.startDematerialization);
    }
    private void ProjectableObjectDematerialization_OnEndDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        StopAudioSource();
    }

}
