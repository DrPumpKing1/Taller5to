using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMaterialSmoothVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Renderer _renderer;

    [Header("Settings")]
    [SerializeField, ColorUsage(true, true)] Color emissionColor;
    [SerializeField, Range(0.01f, 10f)] private float smoothColorFactorPower;
    [SerializeField, Range(1f, 10f)] private float smoothColorFactorDePower;

    private Material _material;
    private float powerIntensity;
    private Color baseColor;

    private float targetIntensity;
    private float currentIntensity;
    private float currentSmoothColorFactor;

    private const float INTENSITY_THRESHOLD = 0.005f;

    private void OnEnable()
    {
        ProjectionPlatformProjection.OnAnyStartProjection += ProjectionPlatformProjection_OnAnyStartProjection;
        ProjectionPlatformProjection.OnAnyEndProjection += ProjectionPlatformProjection_OnAnyEndProjection;

        ProjectableObjectDematerialization.OnAnyStartDematerialization += ProjectableObjectDematerialization_OnAnyStartDematerialization;
        ProjectableObjectDematerialization.OnAnyEndDematerialization += ProjectableObjectDematerialization_OnAnyEndDematerialization;

        ProjectionResetObject.OnAnyStartProjectionResetObjectUse += ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse += ProjectionResetObject_OnAnyEndProjectionResetObjectUse;

        LearningPlatformLearn.OnStartLearning += LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning += LearningPlatformLearn_OnEndLearning;
    }

    private void OnDisable()
    {
        ProjectionPlatformProjection.OnAnyStartProjection -= ProjectionPlatformProjection_OnAnyStartProjection;
        ProjectionPlatformProjection.OnAnyEndProjection -= ProjectionPlatformProjection_OnAnyEndProjection;

        ProjectableObjectDematerialization.OnAnyStartDematerialization -= ProjectableObjectDematerialization_OnAnyStartDematerialization;
        ProjectableObjectDematerialization.OnAnyEndDematerialization -= ProjectableObjectDematerialization_OnAnyEndDematerialization;

        ProjectionResetObject.OnAnyStartProjectionResetObjectUse -= ProjectionResetObject_OnAnyStartProjectionResetObjectUse;
        ProjectionResetObject.OnAnyEndProjectionResetObjectUse -= ProjectionResetObject_OnAnyEndProjectionResetObjectUse;

        LearningPlatformLearn.OnStartLearning -= LearningPlatformLearn_OnStartLearning;
        LearningPlatformLearn.OnEndLearning -= LearningPlatformLearn_OnEndLearning;
    }

    private void Awake()
    {
        _material = _renderer.material;
    }

    private void Start()
    {
        GetBaseColorAndIntensity();
        SetNormalIntensity();
        SetIntensity(0f);
    }

    private void Update()
    {
        HandleMaterialIntensity();
    }

    private void GetBaseColorAndIntensity()
    {
        powerIntensity = Mathf.Max(emissionColor.r, emissionColor.g, emissionColor.b);
        baseColor = powerIntensity > 0 ? emissionColor / powerIntensity : Color.black;
    }

    private void HandleMaterialIntensity()
    {
        if (Mathf.Abs(currentIntensity - targetIntensity) <= INTENSITY_THRESHOLD) return;

        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, currentSmoothColorFactor * Time.deltaTime);
        SetIntensity(currentIntensity);
    }

    private void SetNormalIntensity()
    {
        currentSmoothColorFactor = smoothColorFactorDePower;
        targetIntensity = 0f;
    }

    private void SetPowerIntensity()
    {
        currentSmoothColorFactor = smoothColorFactorPower;
        targetIntensity = powerIntensity;
    }

    private void SetIntensity(float intensity) => GeneralRenderingMethods.SetMaterialEmissionColor(_material, baseColor, intensity);


    #region ProjectionPlatformProjection Subscriptions
    private void ProjectionPlatformProjection_OnAnyStartProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        SetPowerIntensity();
    }

    private void ProjectionPlatformProjection_OnAnyEndProjection(object sender, ProjectionPlatformProjection.OnProjectionEventArgs e)
    {
        SetNormalIntensity();
    }
    #endregion

    #region ProjectableObjectDematerialization Subscriptions
    private void ProjectableObjectDematerialization_OnAnyStartDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        SetPowerIntensity();
    }
    private void ProjectableObjectDematerialization_OnAnyEndDematerialization(object sender, ProjectableObjectDematerialization.OnAnyObjectDematerializedEventArgs e)
    {
        SetNormalIntensity();
    }
    #endregion

    #region ProjectionResetObject Subscriptions
    private void ProjectionResetObject_OnAnyStartProjectionResetObjectUse(object sender, System.EventArgs e)
    {
        SetPowerIntensity();
    }
    private void ProjectionResetObject_OnAnyEndProjectionResetObjectUse(object sender, System.EventArgs e)
    {
        SetNormalIntensity();
    }
    #endregion

    #region LearningPlatformLearn Subscriptions
    private void LearningPlatformLearn_OnStartLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        SetPowerIntensity();
    }
    private void LearningPlatformLearn_OnEndLearning(object sender, LearningPlatformLearn.OnLearningEventArgs e)
    {
        SetNormalIntensity();
    }
    #endregion
}
