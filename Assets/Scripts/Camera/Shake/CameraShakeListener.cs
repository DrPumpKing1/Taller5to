using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeListener : MonoBehaviour
{
    [System.Serializable]
    public class CameraShakeSettings
    {
        [Range(0, 2f)] public float amplitude;
        [Range(0, 5f)] public float frequency;
        [Range(0.5f, 10f)] public float shakeTime;
        [Range(0f, 10f)] public float fadeInTime;
        [Range(0f, 10f)] public float fadeOutTime;
    }

    [Header("Shakes Settings")]
    [SerializeField] private CameraShakeSettings virtueDoorSettings;
    [SerializeField] private CameraShakeSettings bossPhaseChangeSettings;
    [SerializeField] private CameraShakeSettings bossAlmostDefeatedSettings;
    [SerializeField] private CameraShakeSettings bossDefeatedSettings;

    private void OnEnable()
    {
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossAlmostDefeated += BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }


 

    private void OnDisable()
    {
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossAlmostDefeated -= BossStateHandler_OnBossAlmostDefeated;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
    }

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e) => CameraShakeHandler.Instance.ShakeCamera(virtueDoorSettings.amplitude, virtueDoorSettings.frequency, virtueDoorSettings.shakeTime, virtueDoorSettings.fadeInTime, virtueDoorSettings.fadeOutTime);
    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossPhaseChangeSettings.amplitude, bossPhaseChangeSettings.frequency, bossPhaseChangeSettings.shakeTime, bossPhaseChangeSettings.fadeInTime, bossPhaseChangeSettings.fadeOutTime);
    private void BossStateHandler_OnBossAlmostDefeated(object sender, System.EventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossAlmostDefeatedSettings.amplitude, bossAlmostDefeatedSettings.frequency, bossAlmostDefeatedSettings.shakeTime, bossAlmostDefeatedSettings.fadeInTime, bossAlmostDefeatedSettings.fadeOutTime);
    private void BossStateHandler_OnBossDefeated(object sender, System.EventArgs e) => CameraShakeHandler.Instance.ShakeCamera(bossDefeatedSettings.amplitude, bossDefeatedSettings.frequency, bossDefeatedSettings.shakeTime, bossDefeatedSettings.fadeInTime, bossDefeatedSettings.fadeOutTime);
}
