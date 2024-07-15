using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionSFXManager : CustomSFXManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ProjectionPlatformProjection.OnStartProjection += ProjectionPlatformProjection_OnStartProjection;
        ProjectionPlatformProjection.OnEndProjection += ProjectionPlatformProjection_OnEndProjection;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ProjectionPlatformProjection.OnStartProjection -= ProjectionPlatformProjection_OnStartProjection;
        ProjectionPlatformProjection.OnEndProjection -= ProjectionPlatformProjection_OnEndProjection;
    }

    private void ProjectionPlatformProjection_OnStartProjection(object sender, System.EventArgs e)
    {
        ReplaceAudioClip(SFXPoolSO.startProjection);
    }

    private void ProjectionPlatformProjection_OnEndProjection(object sender, System.EventArgs e)
    {
        StopAudioSource();
    }

}
