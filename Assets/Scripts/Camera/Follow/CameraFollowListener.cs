using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowListener : MonoBehaviour
{
    [System.Serializable]
    public class CameraTransition
    {
        public string logToStart;
        public string logToEnd;
        public Transform targetTransform;
        [Range(0.5f, 4f)] public float stallTimeIn;
        [Range(0.5f, 4f)] public float moveInTime;
        [Range(0.5f, 10f)] public float stallTime;
        [Range(0.5f, 4f)] public float moveOutTime;
        [Range(0.5f, 4f)] public float stallTimeOut;
        [Range(2.5f, 8f)] public float targetDistance;
        public bool endInTime;
    }

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

    private void StartTransition(CameraTransition cameraTransition) => CameraFollowHandler.Instance.TransitionMoveCamera(cameraTransition.targetTransform, cameraTransition.stallTimeIn, cameraTransition.moveInTime, cameraTransition.stallTime, cameraTransition.moveOutTime, cameraTransition.stallTimeOut, cameraTransition.targetDistance, cameraTransition.endInTime);
    private void EndTransition(CameraTransition cameraTransition) => CameraFollowHandler.Instance.EndTransition(cameraTransition.targetTransform, cameraTransition.stallTimeIn, cameraTransition.moveInTime, cameraTransition.stallTime, cameraTransition.moveOutTime, cameraTransition.stallTimeOut, cameraTransition.targetDistance, cameraTransition.endInTime);
    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckStartTransition(e.gameplayAction.log);
        CheckEndTransition(e.gameplayAction.log);
    }
    #endregion
}
