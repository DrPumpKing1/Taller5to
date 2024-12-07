using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMaterialVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Renderer _renderer;

    [Header("Settings")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material almostDefeatedMaterial;
    [SerializeField] private Material defeatedMaterial;

    private void OnEnable()
    {
        BossStateHandler.OnBossAlmostDefeated += BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossAlmostDefeated -= BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
    }

    private void Awake()
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, normalMaterial);
    }

    private void BossStateHandler_OnBossAlmostDefeated(object sender, System.EventArgs e)
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, almostDefeatedMaterial);
    }

    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e)
    {
        GeneralRenderingMethods.SetRendererMaterial(_renderer, defeatedMaterial);
    }
}
