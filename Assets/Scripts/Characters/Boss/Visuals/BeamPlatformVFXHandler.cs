using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPlatformVFXHandler : MonoBehaviour
{
    private void OnEnable()
    {
        BossBeam.OnBeamPlatformTargeted += BossBeam_OnBeamPlatformTargeted;
        BossBeam.OnBeamPlatformTargetCleared += BossBeam_OnBeamPlatformTargetCleared;
    }

    private void OnDisable()
    {
        BossBeam.OnBeamPlatformTargeted -= BossBeam_OnBeamPlatformTargeted;
        BossBeam.OnBeamPlatformTargetCleared -= BossBeam_OnBeamPlatformTargetCleared;
    }

    #region BossBeam Subscriptions
    private void BossBeam_OnBeamPlatformTargeted(object sender, BossBeam.OnBeamPlatformTargetEventArgs e)
    {
        
    }
    private void BossBeam_OnBeamPlatformTargetCleared(object sender, BossBeam.OnBeamPlatformTargetEventArgs e)
    {
        
    }
    #endregion
}
