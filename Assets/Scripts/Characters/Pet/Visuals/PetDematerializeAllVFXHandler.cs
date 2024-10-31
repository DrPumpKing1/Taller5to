using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PetDematerializeAllVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect dematerializeAllVFX;

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

    private void InitializeVFX()
    {
        dematerializeAllVFX.gameObject.SetActive(true);
        dematerializeAllVFX.Stop();
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
