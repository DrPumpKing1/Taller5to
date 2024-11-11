using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomWeakpointMaterialVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShowcaseRoomWeakpoint showcaseRoomWeakpoint;
    [SerializeField] private Renderer _renderer;

    [Header("Settings")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private Material unHitMaterial;

    private Material material;

    private void OnEnable()
    {
        showcaseRoomWeakpoint.OnWeakpointHit += ShowcaseRoomWeakpoint_OnWeakpointHit;
        showcaseRoomWeakpoint.OnWeakpointUnHit += ShowcaseRoomWeakpoint_OnWeakpointUnHit;
    }

    private void OnDisable()
    {
        showcaseRoomWeakpoint.OnWeakpointHit -= ShowcaseRoomWeakpoint_OnWeakpointHit;
        showcaseRoomWeakpoint.OnWeakpointUnHit -= ShowcaseRoomWeakpoint_OnWeakpointUnHit;
    }

    private void Awake()
    {
        material = _renderer.material;
        GeneralRenderingMethods.SetRendererMaterial(_renderer, unHitMaterial);
    }

    #region ShowcaseRoomWeakpointSubscriptions
    private void ShowcaseRoomWeakpoint_OnWeakpointHit(object sender, System.EventArgs e)
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, hitMaterial);
    }

    private void ShowcaseRoomWeakpoint_OnWeakpointUnHit(object sender, EventArgs e)
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, unHitMaterial);
    }
    #endregion
}
