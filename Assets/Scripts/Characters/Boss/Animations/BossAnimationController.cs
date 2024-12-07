using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    private const string CHARGING_BOOLEAN = "Charging";
    private const string ALMOST_DEFEATED_BOOLEAN = "AlmostDefeated";
    private const string DEFEATED_BOOLEAN = "Defeated";

    private bool charging;
    private bool almostDefeated;
    private bool defeated;

    private void OnEnable()
    {
        BossStateHandler.OnBossAlmostDefeated += BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossBeam.OnBeamChargeStart += BossBeam_OnBeamChargeStart;
        BossBeam.OnBeamChargeEnd += BossBeam_OnBeamChargeEnd;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossAlmostDefeated -= BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        BossBeam.OnBeamChargeStart -= BossBeam_OnBeamChargeStart;
        BossBeam.OnBeamChargeEnd -= BossBeam_OnBeamChargeEnd;
    }

    private void Start()
    {
        SetCharging(false);
        SetAlmostDefeated(false);
        SetDefeated(false);
    }

    private void Update()
    {
        UpdateAnimatorBooleans();
    }

    private void UpdateAnimatorBooleans()
    {
        animator.SetBool(CHARGING_BOOLEAN, charging);
        animator.SetBool(ALMOST_DEFEATED_BOOLEAN, almostDefeated);
        animator.SetBool(DEFEATED_BOOLEAN, defeated);
    }

    private void SetCharging(bool charging) => this.charging = charging;
    private void SetAlmostDefeated(bool almostDefeated) => this.almostDefeated = almostDefeated;
    private void SetDefeated(bool defeated) => this.defeated = defeated;

    private void BossBeam_OnBeamChargeStart(object sender, BossBeam.OnBeamEventArgs e)
    {
        SetCharging(true);
    }

    private void BossBeam_OnBeamChargeEnd(object sender, BossBeam.OnBeamEventArgs e)
    {
        SetCharging(false);
    }

    private void BossStateHandler_OnBossAlmostDefeated(object sender, System.EventArgs e)
    {
        SetAlmostDefeated(true);
    }

    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e)
    {
        SetDefeated(true);
    }
}
