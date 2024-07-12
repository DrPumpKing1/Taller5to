using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetProjectingVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect projectingVFX;

    [Header("Settings")]
    [SerializeField,Range(3f,6f)] private float velocity;

    private Transform currentAttentionTransform = null;

    private const string DURATION_PROPERTY = "Duration";
    private const string ORIGIN_PROPERTY = "Velocity";
    private const string VELOCITY_PROPERTY = "Velocity";
    private const string NORMALIZED_DIRECTION_PROPERTY = "NormalizedDirection";

    private void OnEnable()
    {
        ProjectionPlatformProjection.OnStartProjection += ProjectionPlatformProjection_OnStartProjection;   
        ProjectionPlatformProjection.OnEndProjection += ProjectionPlatformProjection_OnEndProjection;
    }

    private void OnDisable()
    {
        ProjectionPlatformProjection.OnStartProjection -= ProjectionPlatformProjection_OnStartProjection;
        ProjectionPlatformProjection.OnEndProjection -= ProjectionPlatformProjection_OnEndProjection;
    }

    private void Start()
    {
        InitializeVFX();
    }

    private void Update()
    {
        HandleVFXOrigin();
        HandleVFXDirection();
    }
    private void InitializeVFX()
    {
        projectingVFX.gameObject.SetActive(true);
        projectingVFX.Stop();
    }

    private void HandleVFXOrigin()
    {
        if (!currentAttentionTransform) return;

        Vector3 origin = projectingVFX.transform.position;

        if (projectingVFX.HasVector3(ORIGIN_PROPERTY))
        {
            projectingVFX.SetVector3(ORIGIN_PROPERTY, origin);
        }
    }

    private void HandleVFXDirection()
    {
        if (!currentAttentionTransform) return;

        Vector3 normalizedDirection = (currentAttentionTransform.position - projectingVFX.transform.position).normalized;

        if (projectingVFX.HasVector3(NORMALIZED_DIRECTION_PROPERTY))
        {
            projectingVFX.SetVector3(NORMALIZED_DIRECTION_PROPERTY, normalizedDirection);
        }

    }

    private void SetVFXDuration(float duration)
    {
        if (projectingVFX.HasFloat(DURATION_PROPERTY))
        {
            projectingVFX.SetFloat(DURATION_PROPERTY, duration);
        }
    }

    private void SetVFXVelocity(float velocity)
    {
        if (projectingVFX.HasFloat(VELOCITY_PROPERTY))
        {
            projectingVFX.SetFloat(VELOCITY_PROPERTY, velocity);
        }
    }

    private void ProjectionPlatformProjection_OnStartProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        SetVFXDuration(e.holdDuration);
        SetVFXVelocity(velocity);

        projectingVFX.Play();
        currentAttentionTransform = e.attentionTransform;
    }

    private void ProjectionPlatformProjection_OnEndProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        projectingVFX.Stop();
        currentAttentionTransform = null;
    }
}
