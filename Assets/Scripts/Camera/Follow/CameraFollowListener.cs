using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CinematicsManager;

public class CameraFollowListener : MonoBehaviour
{
    [System.Serializable]
    public class CameraTransition
    {
        public string logToTransition;
        public Transform targetTransform;
        [Range(0.5f, 4f)] public float stallTimeIn;
        [Range(0.5f, 4f)] public float moveInTime;
        [Range(0.5f, 10f)] public float stallTime;
        [Range(0.5f, 4f)] public float moveOutTime;
        [Range(0.5f, 4f)] public float stallTimeOut;
    }

    [Header("Camera Transitions")]
    [SerializeField] private List<CameraTransition> cameraTransitions;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) 
        {
            StartTransition(cameraTransitions[0]);
        }
    }

    private void CheckStartTransition(string log)
    {
        foreach (CameraTransition cameraTransition in cameraTransitions)
        {
            if (cameraTransition.logToTransition == log)
            {
                StartTransition(cameraTransition);
                return;
            }
        }
    }

    private void StartTransition(CameraTransition cameraTransition) => CameraFollowHandler.Instance.TransitionMoveCamera(cameraTransition.targetTransform, cameraTransition.stallTimeIn, cameraTransition.moveInTime, cameraTransition.stallTime, cameraTransition.moveOutTime, cameraTransition.stallTimeOut);

    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckStartTransition(e.gameplayAction.log);
    }
    #endregion
}
