using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetDematerializeAllVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect dematerializeAllVFX;

    private const string SPHERE_WORLD_POSITION_PROPERTY = "SphereWorldPosition";

    private void OnEnable()
    {
        ProjectionResetObject.OnAnyStartProjectionResetObjectUse += ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse += ProjectionResetObject_OnAnyEndProjectionResetObjectUse;
    }

    private void OnDisable()
    {
        ProjectionResetObject.OnAnyStartProjectionResetObjectUse -= ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse -= ProjectionResetObject_OnAnyEndProjectionResetObjectUse;
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
        dematerializeAllVFX.gameObject.SetActive(true);
        dematerializeAllVFX.Stop();
    }
    private void HandleRotation()
    {
        dematerializeAllVFX.transform.rotation = Quaternion.identity;
    }

    private void HandleSphereWorldPosition()
    {
        if (!dematerializeAllVFX.HasVector3(SPHERE_WORLD_POSITION_PROPERTY)) return;

        dematerializeAllVFX.SetVector3(SPHERE_WORLD_POSITION_PROPERTY, transform.position);
    }

    private void StartVFX() => dematerializeAllVFX.Play();
    private void StopVFX() => dematerializeAllVFX.Stop();

    private void ProjectionResetObject_OnAnyStartProjectionResetObjectUse(object sender, EventArgs e)
    {
        StartVFX();
    }

    private void ProjectionResetObject_OnAnyEndProjectionResetObjectUse(object sender, EventArgs e)
    {
        StopVFX();
    }
}
