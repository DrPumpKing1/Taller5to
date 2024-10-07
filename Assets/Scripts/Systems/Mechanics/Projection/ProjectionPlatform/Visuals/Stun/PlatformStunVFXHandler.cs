using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlatformStunVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StunableProjectionPlatformProjection stunableProjectionPlatform;

    private void OnEnable()
    {
        stunableProjectionPlatform.OnProjectionPlatformStun += StunableProjectionPlatform_OnProjectionPlatformStun;
        stunableProjectionPlatform.OnProjectionPlatformEndStun += StunableProjectionPlatform_OnProjectionPlatformEndStun;
    }
    private void OnDisable()
    {
        stunableProjectionPlatform.OnProjectionPlatformStun -= StunableProjectionPlatform_OnProjectionPlatformStun;
        stunableProjectionPlatform.OnProjectionPlatformEndStun -= StunableProjectionPlatform_OnProjectionPlatformEndStun;
    }

    #region StunableProjectionPlatform Subscriptions
    private void StunableProjectionPlatform_OnProjectionPlatformEndStun(object sender, System.EventArgs e)
    {
        
    }

    private void StunableProjectionPlatform_OnProjectionPlatformStun(object sender, System.EventArgs e)
    {
        
    }
    #endregion
}
