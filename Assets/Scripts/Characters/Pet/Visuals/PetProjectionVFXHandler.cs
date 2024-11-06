using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetProjectionVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect projectionVFX;

    private const string SPHERE_WORLD_POSITION_PROPERTY = "SphereWorldPosition";

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
        HandleSphereWorldPosition();
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

    private void HandleSphereWorldPosition()
    {
        if (!projectionVFX.HasVector3(SPHERE_WORLD_POSITION_PROPERTY)) return;

        projectionVFX.SetVector3(SPHERE_WORLD_POSITION_PROPERTY, transform.position);
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
