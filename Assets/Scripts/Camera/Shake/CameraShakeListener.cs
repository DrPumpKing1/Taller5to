using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PetGuidanceListener;

public class CameraShakeListener : MonoBehaviour
{
    [Header("Shakes Settings")]
    [SerializeField] private List<CameraShake> cameraShakes;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void CheckStartCameraShake(string log)
    {
        foreach (CameraShake cameraShake in cameraShakes)
        {
            if (cameraShake.logToStart == log)
            {
                StartCameraShake(cameraShake);
                return;
            }
        }
    }

    private void StartCameraShake(CameraShake cameraShake)
    {
        CameraShakeHandler.Instance.ShakeCamera(cameraShake);
    }

    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckStartCameraShake(e.gameplayAction.log);
    }
    #endregion
}
