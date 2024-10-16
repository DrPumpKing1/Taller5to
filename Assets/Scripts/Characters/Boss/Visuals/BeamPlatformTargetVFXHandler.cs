using System.Collections;
using System.Collections.Generic;
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

    [Header("Settings")]
    [SerializeField, Range(0.05f,0.5f)] private float beamThickness;
    [SerializeField, Range(0f, 1f)] private float bezierPointsDistortion;

    private const string THICKNESS_PROPERTY = "Thickness";
    private const float BEAM_THICKNESS_OFF = 0f;

    private Transform currentTargetProjectableObjectCenter;
    private bool isTargeting;

    private void OnEnable()
    {
        BossBeam.OnBeamPlatformTargeted += BossBeam_OnBeamPlatformTargeted;
        BossBeam.OnBeamPlatformTargetCleared += BossBeam_OnBeamPlatformTargetCleared;
    }

    private void OnDisable()
    {
        BossBeam.OnBeamPlatformTargeted -= BossBeam_OnBeamPlatformTargeted;
        BossBeam.OnBeamPlatformTargetCleared -= BossBeam_OnBeamPlatformTargetCleared;
    }

    private void Start()
    {
        InitializeVFX();
    }

    private void InitializeVFX()
    {
        platformTargetVFX.gameObject.SetActive(true);
        StopPlatformVFX();
        isTargeting = false;
    }

    private void Update()
    {
        HandleFourthBezierPointPosition();
    }

    private void HandleFourthBezierPointPosition()
    {
        if (!isTargeting) return;
        if (!currentTargetProjectableObjectCenter) return;

        position4.position = currentTargetProjectableObjectCenter.position;
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
    #endregion

    private Vector3 GetRandomBezierPointDistortion(float distanceBetween)
    {
        float randomX = Random.Range(0f, bezierPointsDistortion);
        float randomY = Random.Range(0f, bezierPointsDistortion);
        float randomZ = Random.Range(0f, bezierPointsDistortion);

        return new Vector3 (randomX, randomY, randomZ) * distanceBetween;
    }
}
