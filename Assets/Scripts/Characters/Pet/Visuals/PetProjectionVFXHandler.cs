using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetProjectionVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect projectionVFX;

    private void OnEnable()
    {
        ProjectionPlatformProjection.OnAnyStartProjection += ProjectionPlatformProjection_OnAnyStartProjection;
        ProjectionPlatformProjection.OnAnyEndProjection += ProjectionPlatformProjection_OnAnyEndProjection;
    }

    private void OnDisable()
    {
        ProjectionPlatformProjection.OnAnyStartProjection -= ProjectionPlatformProjection_OnAnyStartProjection;
        ProjectionPlatformProjection.OnAnyEndProjection -= ProjectionPlatformProjection_OnAnyEndProjection;
    }

    private void Start()
    {
        InitializeVFX();
    }

    private void Update()
    {
        HandleRotation();
    }

    private void InitializeVFX()
    {
        projectionVFX.gameObject.SetActive(true);
        projectionVFX.Stop();
    }

    private void HandleRotation()
    {
        projectionVFX.transform.rotation = Quaternion.identity;
    }

    private void StartVFX() => projectionVFX.Play();
    private void StopVFX() => projectionVFX.Stop();

    private void ProjectionPlatformProjection_OnAnyStartProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        StartVFX();
    }

    private void ProjectionPlatformProjection_OnAnyEndProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        StopVFX();
    }
}
