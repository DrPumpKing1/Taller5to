using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossShieldMaterialVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private BossShield bossShield;

    [Header("Settings")]
    [SerializeField] private float turnOnSpeed;
    [SerializeField] private float turnOffSpeed;

    [Header("States")]
    [SerializeField] private ShieldVisualState state;

    private enum ShieldVisualState { On, Off, TurningOn, TurningOff }

    private Renderer[] renderers;
    private List<Material> materials = new List<Material>();

    private const string ALPHA_PROPERTY = "_Alpha";
    private const string TILING_MULTIPLIER_PROPERTY = "_TilingMultiplier";

    private const float ON_ALPHA = 1f;
    private const float OFF_ALPHA = 0f;

    private const float ON_TILING_MULTIPLIER = 1f;
    private const float OFF_TILING_MULTIPLIER = 0.25f;

    private const float THRESHOLD = 0.025f;

    private float currentAlpha;
    private float currentTilingMultiplier;

    private void OnEnable()
    {
        bossShield.OnBossShieldActivated += BossShield_OnBossShieldActivated;
        bossShield.OnBossShieldDeactivated += BossShield_OnBossShieldDeactivated;
    }

    private void OnDisable()
    {
        bossShield.OnBossShieldActivated -= BossShield_OnBossShieldActivated;
        bossShield.OnBossShieldDeactivated -= BossShield_OnBossShieldDeactivated;
    }

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        AddRenderersMaterials();
    }

    private void Start()
    {
        SetVisualState(ShieldVisualState.On);
        SetCurrentAlpha(ON_ALPHA);
        SetMaterialsTilingMultiplierByAlpha(ON_ALPHA);
    }

    private void AddRenderersMaterials()
    {
        foreach(Renderer renderer in renderers)
        {
            materials.Add(renderer.material);
        }
    }

    private void Update()
    {
        HandleVisualState();
    }

    private void SetVisualState(ShieldVisualState state) => this.state = state;

    private void HandleVisualState()
    {
        switch (state)
        {
            case ShieldVisualState.On:
                OnLogic();
                break;
            case ShieldVisualState.Off:
                OffLogic();
                break;
            case ShieldVisualState.TurningOn:
                TurningOnLogic();
                break;
            case ShieldVisualState.TurningOff:
                TurningOffLogic();
                break;
        }
    }

    private void OnLogic()
    {
        if(currentAlpha != ON_ALPHA)
        {
            SetMaterialsAlpha(ON_ALPHA);
            SetMaterialsTilingMultiplierByAlpha(ON_ALPHA);
            SetCurrentAlpha(ON_ALPHA); 
        }
    }

    private void OffLogic()
    {
        if (currentAlpha != OFF_ALPHA)
        {
            SetMaterialsAlpha(OFF_ALPHA);
            SetMaterialsTilingMultiplierByAlpha(OFF_ALPHA);
            SetCurrentAlpha(OFF_ALPHA);
        }
    }

    private void TurningOnLogic()
    {
        currentAlpha = Mathf.Lerp(currentAlpha, ON_ALPHA, turnOnSpeed * Time.deltaTime);
        SetMaterialsAlpha(currentAlpha);
        SetMaterialsTilingMultiplierByAlpha(currentAlpha);

        if (ON_ALPHA -currentAlpha < THRESHOLD)
        {
            SetVisualState(ShieldVisualState.On);
        }
    }

    private void TurningOffLogic()
    {
        currentAlpha = Mathf.Lerp(currentAlpha, OFF_ALPHA, turnOffSpeed * Time.deltaTime);
        SetMaterialsAlpha(currentAlpha);
        SetMaterialsTilingMultiplierByAlpha(currentAlpha);

        if (currentAlpha - OFF_ALPHA < THRESHOLD)
        {
            SetVisualState(ShieldVisualState.Off);
        }
    }

    private void SetCurrentAlpha(float alpha) => currentAlpha = alpha;

    private float GetTilingMultiplierByAlpha(float alpha)
    {
        float tilingMultiplier = OFF_TILING_MULTIPLIER + alpha * (ON_TILING_MULTIPLIER - OFF_TILING_MULTIPLIER)/(ON_ALPHA-OFF_ALPHA);
        return tilingMultiplier;
    }

    private void SetMaterialsTilingMultiplierByAlpha(float alpha) => SetMaterialsTilingMultiplier(GetTilingMultiplierByAlpha(alpha));

    private void SetMaterialsTilingMultiplier(float multiplier)
    {
        foreach (Material material in materials)
        {
            if (!material.HasFloat(TILING_MULTIPLIER_PROPERTY)) continue;
            material.SetFloat(TILING_MULTIPLIER_PROPERTY, multiplier);
        }
    }

    private void SetMaterialsAlpha(float alpha)
    {
        foreach(Material material in materials)
        {
            if (!material.HasFloat(ALPHA_PROPERTY)) continue;
            material.SetFloat(ALPHA_PROPERTY, alpha);
        }
    }

    #region BossShield Subscriptions
    private void BossShield_OnBossShieldActivated(object sender, System.EventArgs e)
    {
        if (state == ShieldVisualState.On) return;
        if (state == ShieldVisualState.TurningOn) return;

        SetVisualState(ShieldVisualState.TurningOn);
    }

    private void BossShield_OnBossShieldDeactivated(object sender, System.EventArgs e)
    {
        if (state == ShieldVisualState.Off) return;
        if (state == ShieldVisualState.TurningOff) return;

        SetVisualState(ShieldVisualState.TurningOff);

    }

    #endregion
}
