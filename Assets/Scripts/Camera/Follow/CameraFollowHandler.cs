using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollowHandler : MonoBehaviour
{
    public static CameraFollowHandler Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera CMVCam;
    [SerializeField] private Transform playerCameraFollowPoint;

    [Header("States")]
    [SerializeField] private State state;

    private Vector3 originalPlayerCameraFollowPointPosition;

    public enum State { FollowingPlayer, MovingTransition }

    public State CameraState => state;

    private const float MOVE_TIME_FACTOR = 0.1f;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetCameraState(State.FollowingPlayer);
        InitializeVariables();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CameraFollowPointHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        originalPlayerCameraFollowPointPosition = playerCameraFollowPoint.localPosition;
    }

    private void SetCameraState(State state) => this.state = state;

    public void TransitionMoveCamera(Transform targetTransform, float stallTimeIn, float moveInTime, float stallTime, float moveOutTime, float stallTimeOut)
    {
        if (state != State.FollowingPlayer) return;

        StopAllCoroutines();
        StartCoroutine(TransitionMoveCameraCoroutine(targetTransform, stallTimeIn, moveInTime, stallTime, moveOutTime, stallTimeOut));
    }

    private IEnumerator TransitionMoveCameraCoroutine(Transform targetTransform, float stallTimeIn, float moveInTime, float stallTime, float moveOutTime,  float stallTimeOut)
    {
        SetCameraState(State.MovingTransition);

        GameObject cameraFollowGameObject = new GameObject("CameraFollowGameObject");
        Transform cameraFollowTransform = cameraFollowGameObject.transform;

        cameraFollowTransform.position = playerCameraFollowPoint.position;
        CMVCam.Follow = cameraFollowTransform;

        Vector3 startingPositionIn = cameraFollowTransform.position;

        yield return new WaitForSeconds(stallTimeIn);

        float time = 0f;
        float positionDifferenceMagnitude = float.MaxValue;

        while (positionDifferenceMagnitude > 0.2f)
        {
            cameraFollowTransform.position = Vector3.Lerp(cameraFollowTransform.position, targetTransform.position, time/(moveInTime) * 1/(MOVE_TIME_FACTOR * moveInTime) * Time.deltaTime);

            positionDifferenceMagnitude = (cameraFollowTransform.position - targetTransform.position).magnitude;
            time += Time.deltaTime;
            yield return null;
        }

        cameraFollowTransform.position = targetTransform.position;

        yield return new WaitForSeconds(stallTime);

        time = 0f;
        positionDifferenceMagnitude = float.MaxValue;

        while (positionDifferenceMagnitude > 0.2f)
        {
            cameraFollowTransform.position = Vector3.Lerp(cameraFollowTransform.position, playerCameraFollowPoint.position, time / (moveOutTime) * 1 / (MOVE_TIME_FACTOR * moveOutTime) * Time.deltaTime);

            positionDifferenceMagnitude = (cameraFollowTransform.position - playerCameraFollowPoint.position).magnitude;
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;

        while(time<=  stallTimeOut) //To recftify position during stallTimeOut
        {
            cameraFollowTransform.position = playerCameraFollowPoint.position;
            time += Time.deltaTime;
            yield return null;
        }

        cameraFollowTransform.position = playerCameraFollowPoint.position;
        CMVCam.Follow = playerCameraFollowPoint;

        Destroy(cameraFollowGameObject);

        SetCameraState(State.FollowingPlayer);
    }
}
