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

    public enum State { FollowingPlayer, MovingIn, LookingTarget, MovingOut }

    public State CameraState => state;

    private const float MOVE_TIME_FACTOR = 0.1f;

    private Transform currentCameraFollowTransform;
    private float previousCameraDistance;

    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionInStart;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionInEnd;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionOutStart;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionOutEnd;

    public class OnCameraTransitionEventArgs : EventArgs
    {
        
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
        SetCameraState(State.FollowingPlayer);
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

    private void SetCurrentCameraFollowTransform(Transform transform) => currentCameraFollowTransform = transform;
    private void ClearCurrentCameraFollowTransform() => currentCameraFollowTransform = null;

    public void TransitionMoveCamera(Transform targetTransform, float stallTimeIn, float moveInTime, float stallTime, float moveOutTime, float stallTimeOut, float targetDistance, bool endInTime)
    {
        if (state != State.FollowingPlayer) return;

        StopAllCoroutines();
        StartCoroutine(TransitionMoveCameraCoroutine(targetTransform, stallTimeIn, moveInTime, stallTime, moveOutTime, stallTimeOut, targetDistance, endInTime));
    }

    public void EndTransition(Transform targetTransform, float stallTimeIn, float moveInTime, float stallTime, float moveOutTime, float stallTimeOut, float targetDistance, bool endInTime)
    {
        if (state == State.MovingOut) return;
        if (state == State.FollowingPlayer) return;

        StopAllCoroutines();
        StartCoroutine(EndTransitionCoroutine(targetTransform, stallTimeIn, moveInTime, stallTime, moveOutTime, stallTimeOut, targetDistance, endInTime));
    }


    private IEnumerator TransitionMoveCameraCoroutine(Transform targetTransform, float stallTimeIn, float moveInTime, float stallTime, float moveOutTime,  float stallTimeOut, float targetDistance, bool endInTime)
    {
        SetCameraState(State.MovingIn);

        GameObject cameraFollowGameObject = new GameObject("CameraFollowGameObject");
        Transform cameraFollowTransform = cameraFollowGameObject.transform;

        SetCurrentCameraFollowTransform(cameraFollowTransform);

        currentCameraFollowTransform.position = playerCameraFollowPoint.position;
        CMVCam.Follow = currentCameraFollowTransform;

        Vector3 startingPositionIn = currentCameraFollowTransform.position;

        yield return new WaitForSeconds(stallTimeIn);

        float time = 0f;
        float positionDifferenceMagnitude = float.MaxValue;

        while (positionDifferenceMagnitude > 0.2f)
        {
            currentCameraFollowTransform.position = Vector3.Lerp(currentCameraFollowTransform.position, targetTransform.position, time/(moveInTime) * 1/(MOVE_TIME_FACTOR * moveInTime) * Time.deltaTime);

            positionDifferenceMagnitude = (currentCameraFollowTransform.position - targetTransform.position).magnitude;
            time += Time.deltaTime;
            yield return null;
        }

        currentCameraFollowTransform.position = targetTransform.position;

        SetCameraState(State.LookingTarget);

        if (!endInTime) yield break;

        yield return new WaitForSeconds(stallTime);

        yield return StartCoroutine(EndTransitionCoroutine(targetTransform, stallTimeIn, moveInTime, stallTime, moveOutTime, stallTimeOut, targetDistance, endInTime));
    }

    private IEnumerator EndTransitionCoroutine(Transform targetTransform, float stallTimeIn, float moveInTime, float stallTime, float moveOutTime, float stallTimeOut, float targetDistance, bool endInTime)
    {
        if (state == State.MovingOut) yield break;
        if (state == State.FollowingPlayer) yield break;

        SetCameraState(State.MovingOut);

        float time = 0f;
        float positionDifferenceMagnitude = float.MaxValue;

        while (positionDifferenceMagnitude > 0.2f)
        {
            currentCameraFollowTransform.position = Vector3.Lerp(currentCameraFollowTransform.position, playerCameraFollowPoint.position, time / (moveOutTime) * 1 / (MOVE_TIME_FACTOR * moveOutTime) * Time.deltaTime);

            positionDifferenceMagnitude = (currentCameraFollowTransform.position - playerCameraFollowPoint.position).magnitude;
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;

        while (time <= stallTimeOut) //To rectify position during stallTimeOut
        {
            currentCameraFollowTransform.position = playerCameraFollowPoint.position;
            time += Time.deltaTime;
            yield return null;
        }

        currentCameraFollowTransform.position = playerCameraFollowPoint.position;
        CMVCam.Follow = playerCameraFollowPoint;

        Destroy(currentCameraFollowTransform.gameObject);
        ClearCurrentCameraFollowTransform();

        SetCameraState(State.FollowingPlayer);
    }
}
