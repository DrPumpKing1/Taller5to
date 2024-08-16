using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeListener : MonoBehaviour
{
    [System.Serializable]
    public class CameraShakeSettings
    {
        [Range(0, 1f)] public float amplitude;
        [Range(0, 5f)] public float frequency;
        [Range(0.5f, 10f)] public float shakeTime;
        [Range(0f, 10f)] public float fadeInTime;
        [Range(0f, 10f)] public float fadeOutTime;
    }

    [Header("Shakes Settings")]
    [SerializeField] private CameraShakeSettings virtueDoorSettings;
    [SerializeField] private CameraShakeSettings bossDoorSettings;
    [SerializeField] private CameraShakeSettings bossActiveSettings;
    [SerializeField] private CameraShakeSettings bossPhaseChangeSettings;
    [SerializeField] private CameraShakeSettings bossDefeatedSettings;

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

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e) => CameraShakeHandler.Instance.ShakeCamera(virtueDoorSettings.amplitude, virtueDoorSettings.frequency, virtueDoorSettings.shakeTime, virtueDoorSettings.fadeInTime, virtueDoorSettings.fadeOutTime);
    private void BossDoor_OnBossDoorOpen(object sender, System.EventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossDoorSettings.amplitude, bossDoorSettings.frequency, bossDoorSettings.shakeTime, bossDoorSettings.fadeInTime, bossDoorSettings.fadeOutTime);
    private void BossStateHandler_OnBossActiveStart(object sender, System.EventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossActiveSettings.amplitude, bossActiveSettings.frequency, bossActiveSettings.shakeTime, bossActiveSettings.fadeInTime, bossActiveSettings.fadeOutTime);
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossPhaseChangeSettings.amplitude, bossPhaseChangeSettings.frequency, bossPhaseChangeSettings.shakeTime, bossPhaseChangeSettings.fadeInTime, bossPhaseChangeSettings.fadeOutTime);
    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossDefeatedSettings.amplitude, bossDefeatedSettings.frequency, bossDefeatedSettings.shakeTime, bossDefeatedSettings.fadeInTime, bossDefeatedSettings.fadeOutTime);
}
