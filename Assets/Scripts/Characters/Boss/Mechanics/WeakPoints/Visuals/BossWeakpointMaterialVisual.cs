using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakpointMaterialVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BossWeakPoint bossWeakpoint;
    [SerializeField] private Renderer _renderer;

    [Header("Settings")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private Material unHitMaterial;

    private Material material;

    private void OnEnable()
    {
        bossWeakpoint.OnWeakpointHit += BossWeakpoint_OnWeakpointHit;
        bossWeakpoint.OnWeakpointUnHit += BossWeakpoint_OnWeakpointUnHit;
    }

    private void OnDisable()
    {
        bossWeakpoint.OnWeakpointHit -= BossWeakpoint_OnWeakpointHit;
        bossWeakpoint.OnWeakpointUnHit -= BossWeakpoint_OnWeakpointUnHit;
    }

    private void Awake()
    {
        material = _renderer.material;
        GeneralRenderingMethods.SetRendererMaterial(_renderer, unHitMaterial);
    }

    #region ShowcaseRoomWeakpointSubscriptions
    private void BossWeakpoint_OnWeakpointHit(object sender, System.EventArgs e)
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, hitMaterial);
    }

    private void BossWeakpoint_OnWeakpointUnHit(object sender, EventArgs e)
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, unHitMaterial);
    }
    #endregion
}
