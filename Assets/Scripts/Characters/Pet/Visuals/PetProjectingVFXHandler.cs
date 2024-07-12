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
    [SerializeField,Range(1f,3f)] private float randomLifetimeRatio;

    private Transform currentAttentionTransform = null;

    private const string DURATION_PROPERTY = "Duration";
    private const string MIN_LIFETIME_PROPERTY = "MinLifetime";
    private const string MAX_LIFETIME_PROPERTY = "MaxLifetime";
    private const string VELOCITY_PROPERTY = "Velocity";

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
        //HandleVFXRotation();
        HandleVFXLifetime();
    }
    private void InitializeVFX()
    {
        projectingVFX.gameObject.SetActive(true);
        projectingVFX.Stop();
    }

    private void HandleVFXRotation()
    {
        if (!currentAttentionTransform) return;

        Vector3 direction = (currentAttentionTransform.position - projectingVFX.transform.position).normalized;
        projectingVFX.transform.rotation = Quaternion.LookRotation(direction);
    }

    private void HandleVFXLifetime()
    {
        if (!currentAttentionTransform) return;

        float distanceToAttentionTransform = Vector3.Distance(currentAttentionTransform.position, projectingVFX.transform.position);

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
