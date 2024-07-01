using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeListener : MonoBehaviour
{
    [Header("Virtue Door Settings")]
    [SerializeField,Range(0,1f)] private float virtueDoorShakeAmplitude;
    [SerializeField,Range(0,5f)] private float virtueDoorShakeFrequency;
    [SerializeField,Range(0.5f,10f)] private float virtueDoorShakeTime;
    [SerializeField,Range(0f,10f)] private float virtueDoorShakeFadeInTime;
    [SerializeField,Range(0f,10f)] private float virtueDoorShakeFadeOutTime;

    [Header("Boss Door Settings")]
    [SerializeField, Range(0, 1f)] private float bossDoorShakeAmplitude;
    [SerializeField, Range(0, 5f)] private float bossDoorShakeFrequency;
    [SerializeField, Range(0.5f, 10f)] private float bossDoorShakeTime;
    [SerializeField, Range(0f, 10f)] private float bossDoorShakeFadeInTime;
    [SerializeField, Range(0f, 10f)] private float bossDoorShakeFadeOutTime;

    [Header("Boss Active Settings")]
    [SerializeField, Range(0, 1f)] private float bossActiveShakeAmplitude;
    [SerializeField, Range(0, 5f)] private float bossActiveShakeFrequency;
    [SerializeField, Range(0.5f, 10f)] private float bossActiveShakeTime;
    [SerializeField, Range(0f, 10f)] private float bossActiveShakeFadeInTime;
    [SerializeField, Range(0f, 10f)] private float bossActiveShakeFadeOutTime;

    [Header("Boss PhaseChange Settings")]
    [SerializeField, Range(0, 1f)] private float bossPhaseChangeShakeAmplitude;
    [SerializeField, Range(0, 5f)] private float bossPhaseChangeShakeFrequency;
    [SerializeField, Range(0.5f, 10f)] private float bossPhaseChangeShakeTime;
    [SerializeField, Range(0f, 10f)] private float bossPhaseChangeShakeFadeInTime;
    [SerializeField, Range(0f, 10f)] private float bossPhaseChangeShakeFadeOutTime;

    [Header("Boss Defeated Settings")]
    [SerializeField, Range(0, 1f)] private float bossDefeatedShakeAmplitude;
    [SerializeField, Range(0, 5f)] private float bossDefeatedShakeFrequency;
    [SerializeField, Range(0.5f, 10f)] private float bossDefeatedShakeTime;
    [SerializeField, Range(0f, 10f)] private float bossDefeatedShakeFadeInTime;
    [SerializeField, Range(0f, 10f)] private float bossDefeatedShakeFadeOutTime;

    private void OnEnable()
    {
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
        //BossDoor.OnBossDoorOpen += BossDoor_OnBossDoorOpen;
        BossStateHandler.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    private void OnDisable()
    {
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
        //BossDoor.OnBossDoorOpen -= BossDoor_OnBossDoorOpen;
        BossStateHandler.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
    }




    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e) => CameraShakeHandler.Instance.ShakeCamera(virtueDoorShakeAmplitude, virtueDoorShakeFrequency, virtueDoorShakeTime, virtueDoorShakeFadeInTime, virtueDoorShakeFadeOutTime);
    private void BossDoor_OnBossDoorOpen(object sender, System.EventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossDoorShakeAmplitude, bossDoorShakeFrequency, bossDoorShakeTime, bossDoorShakeFadeInTime, bossDoorShakeFadeOutTime);
    private void BossStateHandler_OnBossActiveStart(object sender, System.EventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossActiveShakeAmplitude, bossActiveShakeFrequency, bossActiveShakeTime, bossActiveShakeFadeInTime, bossActiveShakeFadeOutTime);
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossPhaseChangeShakeAmplitude, bossPhaseChangeShakeFrequency, bossPhaseChangeShakeTime, bossPhaseChangeShakeFadeInTime, bossPhaseChangeShakeFadeOutTime);
    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossDefeatedShakeAmplitude, bossDefeatedShakeFrequency, bossDefeatedShakeTime, bossDefeatedShakeFadeInTime, bossDefeatedShakeFadeOutTime);
}
