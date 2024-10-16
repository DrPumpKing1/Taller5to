using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BeamPlatformDematerializationVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect platformDematerializationVFX;

    [Header("Settings")]
    [SerializeField, Range(10f, 20f)] private float lightningHeight;
    [SerializeField, Range(0.1f, 2f)] private float lightningLifetime;

    private const string LIGHTNING_HEIGHT_PROPERTY = "LightningHeight";
    private const string LIGHTNING_LIFETIME_PROPERTY = "LightningLifeTime";
    private void OnEnable()
    {
        BossBeam.OnBeamObjectDematerialization += BossBeam_OnBeamObjectDematerialization;
    }

    private void OnDisable()
    {
        BossBeam.OnBeamObjectDematerialization -= BossBeam_OnBeamObjectDematerialization;
    }


    private void Start()
    {
        InitializeVFX();
    }

    private void InitializeVFX()
    {
        InitializeFloatProperty(LIGHTNING_HEIGHT_PROPERTY,lightningHeight);
        InitializeFloatProperty(LIGHTNING_LIFETIME_PROPERTY, lightningLifetime);
        platformDematerializationVFX.Stop();
    }

    private void InitializeFloatProperty(string propertyName, float propertyValue)
    {
        if (!platformDematerializationVFX.HasFloat(propertyName)) return;

        platformDematerializationVFX.SetFloat(propertyName, propertyValue);
    }

    private void RelocateLightningOrigin(Vector3 position)
    {
        Vector3 newOrigin = new Vector3(position.x, position.y + lightningHeight, position.z);
        platformDematerializationVFX.transform.position = newOrigin;
    }

    private void DematerializePlatformVFX(Vector3 position)
    {
        RelocateLightningOrigin(position);
        platformDematerializationVFX.Play();
    }

    #region BossBeam Subscriptions
    private void BossBeam_OnBeamObjectDematerialization(object sender, BossBeam.OnBeamObjectDematerializationEventArgs e)
    {
        DematerializePlatformVFX(e.stunableProjectionPlatformProjection.transform.position);
    }
    #endregion
}
