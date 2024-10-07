using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSphereVFXHandler : MonoBehaviour
{
    private void OnEnable()
    {
        BossBeam.OnBeamSphereSelected += BossBeam_OnBeamSphereSelected;
        BossBeam.OnBeamSphereCleared += BossBeam_OnBeamSphereCleared;
    }

    private void OnDisable()
    {
        BossBeam.OnBeamSphereSelected -= BossBeam_OnBeamSphereSelected;
        BossBeam.OnBeamSphereCleared -= BossBeam_OnBeamSphereCleared;
    }

    #region BossBeam Subscriptions
    private void BossBeam_OnBeamSphereSelected(object sender, BossBeam.OnBeamSphereEventArgs e)
    {
        
    }
    private void BossBeam_OnBeamSphereCleared(object sender, BossBeam.OnBeamSphereEventArgs e)
    {
        
    }
    #endregion
}
