using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomListener : MonoBehaviour
{
    [Header("Camera Transitions")]
    [SerializeField] private List<CameraZoom> cameraZooms;

    private CameraZoom currentCameraZoom;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
        CameraTransitionHandler.OnCameraTransitionInStart += CameraTransitionHandler_OnCameraTransitionInStart;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
        CameraTransitionHandler.OnCameraTransitionInStart -= CameraTransitionHandler_OnCameraTransitionInStart;
    }

    private void Start()
    {
        ClearCurrentCameraZoom();
    }

    private void CheckStartZoom(string log)
    {
        foreach (CameraZoom cameraZoom in cameraZooms)
        {
            if (cameraZoom.logToStart == log)
            {
                SetCurrentCameraZoom(cameraZoom);
                StartZoom(currentCameraZoom);
                return;
            }
        }
    }

    private void CheckEndZoom(string log)
    {
        if (currentCameraZoom == null) return;

        if (currentCameraZoom.logToEnd == log)
        {
            EndZoom(currentCameraZoom);
            ClearCurrentCameraZoom();
        }
    }

    private void SetCurrentCameraZoom(CameraZoom cameraZoom) => currentCameraZoom = cameraZoom;
    private void ClearCurrentCameraZoom() => currentCameraZoom = null;

    private void StartZoom(CameraZoom cameraZoom) => CameraZoomHandler.Instance.ZoomCamera(cameraZoom);
    private void EndZoom(CameraZoom cameraZoom) => CameraZoomHandler.Instance.EndZoom(cameraZoom);
    private void CancelZoom()
    {
        ClearCurrentCameraZoom();
        CameraZoomHandler.Instance.CancelZoom();
    }

    #region GameLogManagerSubscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckStartZoom(e.gameplayAction.log);
        CheckEndZoom(e.gameplayAction.log);
    }
    #endregion

    #region CameraTransitionHandler Subscriptions
    private void CameraTransitionHandler_OnCameraTransitionInStart(object sender, CameraTransitionHandler.OnCameraTransitionEventArgs e)
    {
        CancelZoom();
    }
    #endregion
}
