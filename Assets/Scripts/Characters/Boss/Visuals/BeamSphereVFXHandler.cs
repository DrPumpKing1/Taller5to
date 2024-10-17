using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BeamSphereVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VisualEffect beamSphereVFX;

    [Header("Sphere Size")]
    [SerializeField, Range(0.5f, 1f)] private float startSphereSize;
    [SerializeField, Range(0.5f, 2f)] private float endSphereSize;

    private const string SPHERE_SIZE_PROPERTY = "SphereRadius";

    private bool isCharging;

    private float chargeTime;
    private float timeCharging;

    private void OnEnable()
    {
        BossBeam.OnBeamChargeStart += BossBeam_OnBeamChargeStart;
        BossBeam.OnBeamChargeEnd += BossBeam_OnBeamChargeEnd;  
    }

    private void OnDisable()
    {
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
        HandleChargeTime();

        HandleSphereSize();
    }

    private void InitializeVFX()
    {
        beamSphereVFX.gameObject.SetActive(true);
        StopVFX();
        isCharging = false;
    }

    private void InitializeVariables()
    {
        SetChargeTime(0f);
        ResetTimer();
    }
    private void HandleChargeTime()
    {
        if (!isCharging) return;

        if (timeCharging < chargeTime) timeCharging += Time.deltaTime;
    }

    #region Lerp Handlers
    private void HandleSphereSize()
    {
        if (!isCharging) return;
        if (chargeTime <= 0) return;

        if (!beamSphereVFX.HasFloat(SPHERE_SIZE_PROPERTY)) return;

        float sphereRadius = Mathf.Lerp(startSphereSize, endSphereSize, timeCharging / chargeTime);

        beamSphereVFX.SetFloat(SPHERE_SIZE_PROPERTY, sphereRadius);
    }
    #endregion

    private void StartVFX(Vector3 beamSpherePos)
    {
        beamSphereVFX.Play();
        RepositionVFX(beamSpherePos);
    }
    private void StopVFX() => beamSphereVFX.Stop();

    private void RepositionVFX(Vector3 position) => beamSphereVFX.transform.position = position;

    private void SetChargeTime(float time) => chargeTime = time;
    private void ResetTimer() => timeCharging = 0;

    #region BossBeam Subscriptions
    private void BossBeam_OnBeamChargeStart(object sender, BossBeam.OnBeamEventArgs e)
    {
        SetChargeTime(e.phaseBeam.chargeTime);
        isCharging = true;

        StartVFX(e.beamSphere.position);
    }
    private void BossBeam_OnBeamChargeEnd(object sender, BossBeam.OnBeamEventArgs e)
    {
        SetChargeTime(0f);
        ResetTimer();
        isCharging = false;

        StopVFX();
    }
    #endregion
}
