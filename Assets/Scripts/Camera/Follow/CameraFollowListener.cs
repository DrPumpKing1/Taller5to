using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowListener : MonoBehaviour
{
    [Header("Camera Transitions")]
    [SerializeField] private List<CameraTransition> cameraTransitions;

    private CameraTransition currentCameraTransition;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Start()
    {
        ClearCurrentCameraTransition();
    }

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameLogManager.Instance.Log("test");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameLogManager.Instance.Log("test2");
        }
    }

    private void CheckStartTransition(string log)
    {
        foreach (CameraTransition cameraTransition in cameraTransitions)
        {
            if (cameraTransition.logToStart == log)
            {
                SetCurrentCameraTransition(cameraTransition);
                StartTransition(currentCameraTransition);
                return;
            }
        }
    }

    private void CheckEndTransition(string log)
    {
        if (currentCameraTransition == null) return;

        if(currentCameraTransition.logToEnd == log)
        {
            EndTransition(currentCameraTransition);
            ClearCurrentCameraTransition();
        }
    }

    private void SetCurrentCameraTransition(CameraTransition cameraTransition) => currentCameraTransition = cameraTransition;
    private void ClearCurrentCameraTransition() => currentCameraTransition = null;

    private void StartTransition(CameraTransition cameraTransition) => CameraFollowHandler.Instance.TransitionMoveCamera(cameraTransition);
    private void EndTransition(CameraTransition cameraTransition) => CameraFollowHandler.Instance.EndTransition(cameraTransition);

    #region GameLogManagerSubscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckStartTransition(e.gameplayAction.log);
        CheckEndTransition(e.gameplayAction.log);
    }
    #endregion
}
