using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomBeamSphereSFXManager : CustomSFXManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ShowcaseRoomBeam.OnBeamChargeStart += ShowcaseRoomBeam_OnBeamChargeStart;
        ShowcaseRoomBeam.OnBeamChargeEnd += ShowcaseRoomBeam_OnBeamChargeEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ShowcaseRoomBeam.OnBeamChargeStart -= ShowcaseRoomBeam_OnBeamChargeStart;
        ShowcaseRoomBeam.OnBeamChargeEnd -= ShowcaseRoomBeam_OnBeamChargeEnd;
    }

    private void ShowcaseRoomBeam_OnBeamChargeStart(object sender, ShowcaseRoomBeam.OnBeamEventArgs e)
    {
        RepositionSFXManager(e.beamSphere.position);
        ReplaceAudioClip(SFXPoolSO.showcaseRoomBeamSphereStart);
    }
    private void ShowcaseRoomBeam_OnBeamChargeEnd(object sender, ShowcaseRoomBeam.OnBeamEventArgs e)
    {
        StopAudioSource();
    }
}
