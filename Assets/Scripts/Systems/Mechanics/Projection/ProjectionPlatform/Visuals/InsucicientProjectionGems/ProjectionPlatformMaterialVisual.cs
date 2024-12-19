using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionPlatformMaterialVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectionPlatform projectionPlatform;
    [SerializeField] private Renderer _renderer;

    [Header("Settings")]
    [SerializeField] private Material notProjectedMaterial;
    [SerializeField] private Material projectedMaterial;

    private void OnEnable()
    {
        projectionPlatform.OnProjectionPlatformSet += ProjectionPlatform_OnProjectionPlatformSet;
        projectionPlatform.OnProjectionPlatformClear += ProjectionPlatform_OnProjectionPlatformClear; 
            
    }

    private void OnDisable()
    {
        projectionPlatform.OnProjectionPlatformSet -= ProjectionPlatform_OnProjectionPlatformSet;
        projectionPlatform.OnProjectionPlatformClear -= ProjectionPlatform_OnProjectionPlatformClear;

    }

    private void Awake()
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, notProjectedMaterial);
    }

    #region ProjectionPlatform Subscriptions
    private void ProjectionPlatform_OnProjectionPlatformSet(object sender, ProjectionPlatform.OnProjectionEventArgs e)
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, projectedMaterial);

    }
    private void ProjectionPlatform_OnProjectionPlatformClear(object sender, System.EventArgs e)
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, notProjectedMaterial);

    }
    #endregion
}
