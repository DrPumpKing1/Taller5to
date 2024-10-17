using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.VFX;

public class BeamPlatformTargetVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect platformTargetVFX;
    [Space]
    [SerializeField] private Transform position1;
    [SerializeField] private Transform position2;
    [SerializeField] private Transform position3;
    [SerializeField] private Transform position4;

    [Header("Lightning Colors")]
    [SerializeField, ColorUsage(true, true)] private Color startLightningColor;
    [SerializeField, ColorUsage(true, true)] private Color endLightningColor;

    [Header("Particcle Colors")]
    [SerializeField, ColorUsage(true, true)] private Color startParticleColor;
    [SerializeField, ColorUsage(true, true)] private Color endParticleColor;

    [Header("Noise")]
    [SerializeField, Range(0.05f, 0.3f)] private float startNoisePower;
    [SerializeField, Range(0.05f, 0.3f)] private float endNoisePower;

    [Header("Thickness")]
    [SerializeField, Range(0.05f,0.2f)] private float startThickness;
    [SerializeField, Range(0.05f,0.2f)] private float endThickness;

    [Header("Bezier Curve")]
    [SerializeField, Range(0f, 1f)] private float startBezierPointsDistortion;
    [SerializeField, Range(0f, 1f)] private float endBezierPointsDistortion;

    private const string LIGHTNING_COLOR_PROPERTY = "Color";
    private const string PARTICLES_COLOR_PROPERTY = "ParticleColor";
    private const string NOISE_POWER_PROPERTY = "NoisePower";
    private const string THICKNESS_PROPERTY = "Thickness";

    private Transform currentTargetProjectableObjectCenter;
    private bool isTargeting;
    private bool isCharging;

    private float chargeTime;
    private float timeCharging;

    private Vector3 position2UnitaryVector;
    private Vector3 position3UnitaryVector;

    private void OnEnable()
    {
        BossBeam.OnBeamPlatformTargeted += BossBeam_OnBeamPlatformTargeted;
        BossBeam.OnBeamPlatformTargetCleared += BossBeam_OnBeamPlatformTargetCleared;

        BossBeam.OnBeamChargeStart += BossBeam_OnBeamChargeStart;
        BossBeam.OnBeamChargeEnd += BossBeam_OnBeamChargeEnd;
    }

    private void OnDisable()
    {
        BossBeam.OnBeamPlatformTargeted -= BossBeam_OnBeamPlatformTargeted;
        BossBeam.OnBeamPlatformTargetCleared -= BossBeam_OnBeamPlatformTargetCleared;

        BossBeam.OnBeamChargeStart -= BossBeam_OnBeamChargeStart;
        BossBeam.OnBeamChargeEnd -= BossBeam_OnBeamChargeEnd;
    }

    private void Start()
    {
        InitializeVFX();
        InitializeVariables();
    }
    private void Update()
    {
        HandleFourthBezierPointPosition();

        HandleChargeTime();

        HandleLighningColor();
        HandleParticleColor();
        HandleNoisePower();
        HandleThickness();
        HandleMiddleBezierPoints();
    }

    private void InitializeVFX()
    {
        platformTargetVFX.gameObject.SetActive(true);
        StopPlatformVFX();
        isTargeting = false;
        isCharging = false;
    }

    private void InitializeVariables()
    {
        SetChargeTime(0f);
        ResetTimer();
    }


    #region Lerp Handlers
    private void HandleFourthBezierPointPosition()
    {
        if (!isTargeting) return;
        if (!currentTargetProjectableObjectCenter) return;

        position4.position = currentTargetProjectableObjectCenter.position;
    }

    private void HandleChargeTime()
    {
        if(!isCharging) return;

        if(timeCharging  < chargeTime) timeCharging += Time.deltaTime;
    }

    private void HandleLighningColor()
    {
        if(!isTargeting) return;
        if(!isCharging) return;
        if (chargeTime <= 0) return;

        if (!platformTargetVFX.HasVector4(LIGHTNING_COLOR_PROPERTY)) return;

        Color color = Color.Lerp(startLightningColor,endLightningColor, timeCharging/chargeTime);

        platformTargetVFX.SetVector4(LIGHTNING_COLOR_PROPERTY, color);
    }

    private void HandleParticleColor()
    {
        if (!isTargeting) return;
        if (!isCharging) return;
        if (chargeTime <= 0) return;

        if (!platformTargetVFX.HasVector4(PARTICLES_COLOR_PROPERTY)) return;

        Color color = Color.Lerp(startParticleColor, endParticleColor, timeCharging / chargeTime);

        platformTargetVFX.SetVector4(PARTICLES_COLOR_PROPERTY, color);
    }

    private void HandleNoisePower()
    {
        if (!isTargeting) return;
        if (!isCharging) return;
        if (chargeTime <= 0) return;

        if (!platformTargetVFX.HasFloat(NOISE_POWER_PROPERTY)) return;

        float noisePower = Mathf.Lerp(startNoisePower, endNoisePower, timeCharging / chargeTime);

        platformTargetVFX.SetFloat(NOISE_POWER_PROPERTY, noisePower);
    }

    private void HandleThickness()
    {
        if (!isTargeting) return;
        if (!isCharging) return;
        if (chargeTime <= 0) return;

        if (!platformTargetVFX.HasFloat(THICKNESS_PROPERTY)) return;

        float thickness = Mathf.Lerp(startThickness, endThickness, timeCharging / chargeTime);

        platformTargetVFX.SetFloat(THICKNESS_PROPERTY, thickness);
    }

    private void HandleMiddleBezierPoints()
    {
        if (!isTargeting) return;
        if (!isCharging) return;
        if (chargeTime <= 0) return;

        float bezierPointsDistortion = Mathf.Lerp(startBezierPointsDistortion, endBezierPointsDistortion, timeCharging / chargeTime);

        Vector3 startPos = position1.position;
        Vector3 endPos = position4.position;

        float distaceBetween = Vector3.Distance(startPos, endPos);

        position2.position = startPos + (endPos - startPos) * 1 / 3 + position2UnitaryVector * distaceBetween * bezierPointsDistortion;
        position3.position = startPos + (endPos - startPos) * 2 / 3 + position2UnitaryVector * distaceBetween * bezierPointsDistortion;
    }

    #endregion

    private void TargetPlatformVFX(Vector3 beamSpherePos, Vector3 projectionPlatformPos)
    {
        platformTargetVFX.Play();

        RepositionEndBezierPoints(beamSpherePos, projectionPlatformPos);
        RepositionMiddleBezierPoints();
        isTargeting = true;
        SetVFX(true);
    }

    private void StopPlatformVFX()
    {
        platformTargetVFX.Stop();
        isTargeting = false;
        SetVFX(false);
    }

    private void RepositionEndBezierPoints(Vector3 startPos, Vector3 endPos)
    {
        position1.position = startPos;
        position4.position = endPos;
    }

    private void RepositionMiddleBezierPoints()
    {
        ChooseMiddleUnitaryVectors();

        Vector3 startPos = position1.position;
        Vector3 endPos = position4.position;

        float distaceBetween = Vector3.Distance(startPos, endPos);

        position2.position = startPos + (endPos - startPos) * 1 / 3 + position2UnitaryVector * distaceBetween * startBezierPointsDistortion;
        position3.position = startPos + (endPos - startPos) * 2 / 3 + position2UnitaryVector * distaceBetween * startBezierPointsDistortion;
    }

    private void ChooseMiddleUnitaryVectors()
    {
        position2UnitaryVector = GetRandomUnitaryVector();
        position3UnitaryVector = GetRandomUnitaryVector();
    }

    private Vector3 GetRandomUnitaryVector()
    {
        float randomX = Random.Range(0f, 1f);
        float randomY = Random.Range(0f, 1f);
        float randomZ = Random.Range(0f, 1f);

        return new Vector3(randomX, randomY, randomZ);
    }

    private void SetCurrentTargetProjectableObjectCenter(Transform target) => currentTargetProjectableObjectCenter = target;
    private void ClearCurrentTargetProjectableObjectCenter() => currentTargetProjectableObjectCenter = null;

    private void SetChargeTime(float time) => chargeTime = time;
    private void ResetTimer() => timeCharging = 0;
    private void SetVFX(bool on) => platformTargetVFX.enabled = on;


    #region BossBeam Subscriptions
    private void BossBeam_OnBeamPlatformTargeted(object sender, BossBeam.OnBeamPlatformTargetEventArgs e)
    {
        TargetPlatformVFX(e.beamSphere.position,e.stunableProjectionPlatformProjection.ProjectionPlatform.CurrentProjectedObject.ProjectableObjectCenter.position);
        SetCurrentTargetProjectableObjectCenter(e.stunableProjectionPlatformProjection.ProjectionPlatform.CurrentProjectedObject.ProjectableObjectCenter);
    }
    private void BossBeam_OnBeamPlatformTargetCleared(object sender, BossBeam.OnBeamPlatformTargetEventArgs e)
    {
        StopPlatformVFX();
        ClearCurrentTargetProjectableObjectCenter();
    }
    private void BossBeam_OnBeamChargeStart(object sender, BossBeam.OnBeamEventArgs e)
    {
        SetChargeTime(e.phaseBeam.chargeTime);
        isCharging = true;
    }

    private void BossBeam_OnBeamChargeEnd(object sender, BossBeam.OnBeamEventArgs e)
    {
        SetChargeTime(0f);
        ResetTimer();
        isCharging = false;
    }
    #endregion
}
