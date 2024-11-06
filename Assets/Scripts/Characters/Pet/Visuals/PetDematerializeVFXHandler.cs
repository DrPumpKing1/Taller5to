using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetDematerializeVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect dematerializeVFX;

    private const string SPHERE_WORLD_POSITION_PROPERTY = "SphereWorldPosition";

    private void OnEnable()
    {
        ProjectableObjectDematerialization.OnAnyStartDematerialization += ProjectableObjectDematerialization_OnAnyStartDematerialization;
        ProjectableObjectDematerialization.OnAnyEndDematerialization += ProjectableObjectDematerialization_OnAnyEndDematerialization;
    }

    private void OnDisable()
    {
        ProjectableObjectDematerialization.OnAnyStartDematerialization -= ProjectableObjectDematerialization_OnAnyStartDematerialization;
        ProjectableObjectDematerialization.OnAnyEndDematerialization -= ProjectableObjectDematerialization_OnAnyEndDematerialization;
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
        dematerializeVFX.gameObject.SetActive(true);
        dematerializeVFX.Stop();
    }

    private void HandleRotation()
    {
        dematerializeVFX.transform.rotation = Quaternion.identity;
    }
    private void HandleSphereWorldPosition()
    {
        if (!dematerializeVFX.HasVector3(SPHERE_WORLD_POSITION_PROPERTY)) return;

        dematerializeVFX.SetVector3(SPHERE_WORLD_POSITION_PROPERTY, transform.position);
    }

    private void StartVFX() => dematerializeVFX.Play();
    private void StopVFX() => dematerializeVFX.Stop();

    private void ProjectableObjectDematerialization_OnAnyStartDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        StartVFX();
    }

    private void ProjectableObjectDematerialization_OnAnyEndDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        StopVFX();
    }
}
