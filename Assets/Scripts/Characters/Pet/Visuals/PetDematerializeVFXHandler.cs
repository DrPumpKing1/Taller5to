using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetDematerializeVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect dematerializeVFX;

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

    private void InitializeVFX()
    {
        dematerializeVFX.gameObject.SetActive(true);
        dematerializeVFX.Stop();
    }

    private void StartVFX() => dematerializeVFX.Play();
    private void StopVFX() => dematerializeVFX.Stop();

    private void ProjectableObjectDematerialization_OnAnyStartDematerialization(object sender, EventArgs e)
    {
        StartVFX();
    }

    private void ProjectableObjectDematerialization_OnAnyEndDematerialization(object sender, EventArgs e)
    {
        StopVFX();
    }
}
