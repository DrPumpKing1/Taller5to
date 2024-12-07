using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSmoothMaterialVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Renderer _renderer;

    [Header("Settings")]
    [SerializeField, ColorUsage(true, true)] private Color normalColor;
    [SerializeField, ColorUsage(true, true)] private Color almostDefeatedColor;
    [SerializeField, ColorUsage(true, true)] private Color defeatedColor;
    [Space]
    [SerializeField, Range(0.01f, 10f)] private float smoothColorFactor;

    private Material _material;

    private float normalIntensity;
    private Color baseNormalColor;

    private float almostDefeatedIntensity;
    private Color baseAlmostDefeatedColor;

    private float defeatedIntensity;
    private Color baseDefeatedColor;

    private float targetIntensity;
    private float currentIntensity;

    private Color targetColor;
    private Color currentColor;

    private const float INTENSITY_THRESHOLD = 0.005f;

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
        _material = _renderer.material;
    }

    private void Start()
    {
        GetBaseColorsAndIntensities();
        SetTargetIntensity(normalIntensity);
        SetTargetColor(baseNormalColor);
        SetColorAndIntensity(baseNormalColor,normalIntensity);
    }

    private void Update()
    {
        HandleMaterialColorAndIntensity();
    }

    private void HandleMaterialColorAndIntensity()
    {
        if (Mathf.Abs(currentIntensity - targetIntensity) <= INTENSITY_THRESHOLD) return;

        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, smoothColorFactor * Time.deltaTime);
        currentColor = Color.Lerp(currentColor, targetColor, smoothColorFactor * Time.deltaTime);

        SetColorAndIntensity(currentColor, currentIntensity);
    }

    private void GetBaseColorsAndIntensities()
    {
        normalIntensity = Mathf.Max(normalColor.r, normalColor.g, normalColor.b);
        baseNormalColor = normalIntensity > 0 ? normalColor / normalIntensity : Color.black;

        almostDefeatedIntensity = Mathf.Max(almostDefeatedColor.r, almostDefeatedColor.g, almostDefeatedColor.b);
        baseAlmostDefeatedColor = almostDefeatedIntensity > 0 ? almostDefeatedColor / almostDefeatedIntensity : Color.black;

        defeatedIntensity = Mathf.Max(defeatedColor.r, defeatedColor.g, defeatedColor.b);
        baseDefeatedColor = defeatedIntensity > 0 ? defeatedColor / defeatedIntensity : Color.black;
    }

    private void SetTargetIntensity(float intensity) => targetIntensity = intensity;
    private void SetTargetColor(Color color) => targetColor = color;
    private void SetColorAndIntensity(Color color, float intensity) => GeneralRenderingMethods.SetMaterialEmissionColor(_material, color, intensity);

    private void BossStateHandler_OnBossAlmostDefeated(object sender, System.EventArgs e)
    {
        SetTargetColor(baseAlmostDefeatedColor);
        SetTargetIntensity(almostDefeatedIntensity);
    }

    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e)
    {
        SetTargetColor(baseDefeatedColor);
        SetTargetIntensity(defeatedIntensity);
    }
}
