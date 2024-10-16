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

    [Header("Settings")]
    [SerializeField, Range(0.05f,0.5f)] private float beamThickness;
    [SerializeField, Range(0f, 1f)] private float bezierPointsDistortion;

    private const string THICKNESS_PROPERTY = "Thickness";
    private const string LIGHTNING_COLOR_PROPERTY = "Color";
    private const string PARTICLES_COLOR_PROPERTY = "ParticleColor";
    private const string NOISE_POWER_PROPERTY = "NoisePower";

    private const float BEAM_THICKNESS_OFF = 0f;

    private Transform currentTargetProjectableObjectCenter;
    private bool isTargeting;
    private bool isCharging;

    private float chargeTime;
    private float timeCharging;

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

    private void InitializeVFX()
    {
        platformTargetVFX.gameObject.SetActive(true);
        StopPlatformVFX();
        isTargeting = false;
    }

    private void InitializeVariables()
    {
        SetChargeTime(0f);
        ResetTimer();
    }

    private void Update()
    {
        HandleFourthBezierPointPosition();

        HandleChargeTime();

        HandleLighningColor();
        HandleParticleColor();
        HandleNoisePower();
    }

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

        float color = Mathf.Lerp(startNoisePower, endNoisePower, timeCharging / chargeTime);

        platformTargetVFX.SetFloat(NOISE_POWER_PROPERTY, color);
    }

    private void SetBeam(bool on)
    {
        if (!platformTargetVFX.HasFloat(THICKNESS_PROPERTY)) return;
        
        if(on)
        {
            platformTargetVFX.SetFloat(THICKNESS_PROPERTY, beamThickness);
        }
        else
        {
            platformTargetVFX.SetFloat(THICKNESS_PROPERTY, BEAM_THICKNESS_OFF);
        }
    }

    private void RepositionBezierPoints(Vector3 startPos, Vector3 endPos)
    {
        float distanceBetween = Vector3.Distance(startPos, endPos);

        position1.position = startPos;
        position4.position = endPos;

        position2.position = startPos + (endPos - startPos) * 1 / 3 + GetRandomBezierPointDistortion(distanceBetween);
        position3.position = startPos + (endPos - startPos) * 2 / 3 + GetRandomBezierPointDistortion(distanceBetween);
    }

    private void TargetPlatformVFX(Vector3 beamSpherePos, Vector3 projectionPlatformPos)
    {
        SetBeam(true);
        platformTargetVFX.Play();

        RepositionBezierPoints (beamSpherePos, projectionPlatformPos);
        isTargeting = true;
    }

    private void StopPlatformVFX()
    {
        SetBeam(false);
        platformTargetVFX.Stop();
        isTargeting = false;
    }

    private void SetCurrentTargetProjectableObjectCenter(Transform target) => currentTargetProjectableObjectCenter = target;
    private void ClearCurrentTargetProjectableObjectCenter() => currentTargetProjectableObjectCenter = null;

    private void SetChargeTime(float time) => chargeTime = time;
    private void ResetTimer() => timeCharging = 0;


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

    private Vector3 GetRandomBezierPointDistortion(float distanceBetween)
    {
        float randomX = Random.Range(0f, bezierPointsDistortion);
        float randomY = Random.Range(0f, bezierPointsDistortion);
        float randomZ = Random.Range(0f, bezierPointsDistortion);

        return new Vector3 (randomX, randomY, randomZ) * distanceBetween;
    }
}
