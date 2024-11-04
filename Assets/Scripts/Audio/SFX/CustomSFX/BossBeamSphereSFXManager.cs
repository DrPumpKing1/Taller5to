using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeamSphereSFXManager : CustomSFXManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
        BossBeam.OnBeamChargeStart += BossBeam_OnBeamChargeStart;
        BossBeam.OnBeamChargeEnd += BossBeam_OnBeamChargeEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        BossBeam.OnBeamChargeStart -= BossBeam_OnBeamChargeStart;
        BossBeam.OnBeamChargeEnd -= BossBeam_OnBeamChargeEnd;
    }

    private void BossBeam_OnBeamChargeStart(object sender, BossBeam.OnBeamEventArgs e)
    {
        RepositionSFXManager(e.beamSphere.position);
        ReplaceAudioClip(SFXPoolSO.bossBeamSphereStart);
    }
    private void BossBeam_OnBeamChargeEnd(object sender, BossBeam.OnBeamEventArgs e)
    {
        StopAudioSource();
    }
}
