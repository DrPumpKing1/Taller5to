using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamPlatformDematerializationVisual : MonoBehaviour
{
    private void OnEnable()
    {
        BossBeam.OnBeamObjectDematerialization += BossBeam_OnBeamObjectDematerialization;
    }

    private void OnDisable()
    {
        BossBeam.OnBeamObjectDematerialization -= BossBeam_OnBeamObjectDematerialization;
    }

    #region BossBeam Subscriptions
    private void BossBeam_OnBeamObjectDematerialization(object sender, BossBeam.OnBeamObjectDematerializationEventArgs e)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
