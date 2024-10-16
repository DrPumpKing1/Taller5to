using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlatformStunVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private StunableProjectionPlatformProjection stunableProjectionPlatform;
    [Space]
    [SerializeField] private GameObject stunnedPlatformModel;
    [SerializeField] private VisualEffect platformStunVFX;

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

    private void Start()
    {
        EndVFX();
    }

    private void StartVFX()
    {
        SetModel(true);
        platformStunVFX.Play();
    }

    private void EndVFX()
    {
        SetModel(false);
        platformStunVFX.Stop();
    }

    private void SetModel(bool enabled) => stunnedPlatformModel.SetActive(enabled);

    #region StunableProjectionPlatform Subscriptions
    private void StunableProjectionPlatform_OnProjectionPlatformStun(object sender, System.EventArgs e)
    {
        StartVFX();
    }

    private void StunableProjectionPlatform_OnProjectionPlatformEndStun(object sender, System.EventArgs e)
    {
        EndVFX();
    }
    #endregion
}
