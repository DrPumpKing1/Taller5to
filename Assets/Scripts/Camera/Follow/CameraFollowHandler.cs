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

    public enum State {FollowingPlayer, StallingIn, MovingIn, LookingTarget, MovingOut, StallingOut}

    public State CameraState => state;

    private const float MOVE_CAMERA_TIME_FACTOR = 0.1f;
    private const float DISTANCE_CAMERA_TIME_FACTOR = 0.08f;

    private Transform currentCameraFollowTransform;
    private float previousCameraDistance;

    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionInStart;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionInEnd;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionOutStart;
    public static event EventHandler<OnCameraTransitionEventArgs> OnCameraTransitionOutEnd;

    public class OnCameraTransitionEventArgs : EventArgs
    {
        public CameraTransition cameraTransition;
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
    private void SetPreviousCameraDistance(float distance) => previousCameraDistance = distance;

    public bool AllowCameraInputProcessing() => state == State.FollowingPlayer || state == State.StallingOut;
    public bool AllowMovementInputProcessing() => state == State.FollowingPlayer || state == State.StallingOut;

    public void TransitionMoveCamera(CameraTransition cameraTransition)
    {
        if (state != State.FollowingPlayer) return;

        StopAllCoroutines();
        StartCoroutine(TransitionMoveCameraCoroutine(cameraTransition));
    }

    public void EndTransition(CameraTransition cameraTransition)
    {
        if (state == State.MovingOut) return;
        if (state == State.FollowingPlayer) return;

        StopAllCoroutines();
        StartCoroutine(EndTransitionCoroutine(cameraTransition));
    }


    private IEnumerator TransitionMoveCameraCoroutine(CameraTransition cameraTransition)
    {
        OnCameraTransitionInStart?.Invoke(this, new OnCameraTransitionEventArgs { cameraTransition = cameraTransition});

        GameObject cameraFollowGameObject = new GameObject("CameraFollowGameObject");
        Transform cameraFollowTransform = cameraFollowGameObject.transform;

        SetCurrentCameraFollowTransform(cameraFollowTransform);
        SetPreviousCameraDistance(CameraScroll.Instance.Distance);

        currentCameraFollowTransform.position = playerCameraFollowPoint.position;
        CMVCam.Follow = currentCameraFollowTransform;

        Vector3 startingPositionIn = currentCameraFollowTransform.position;

        SetCameraState(State.StallingIn);

        yield return new WaitForSeconds(cameraTransition.stallTimeIn);

        SetCameraState(State.MovingIn);

        float time = 0f;
        float positionDifferenceMagnitude = float.MaxValue;
        float distanceDifferenceMagnitude = float.MaxValue;

        while (positionDifferenceMagnitude > 0.2f)
        {
            currentCameraFollowTransform.position = Vector3.Lerp(currentCameraFollowTransform.position, cameraTransition.targetTransform.position, time/(cameraTransition.moveInTime) * 1/(MOVE_CAMERA_TIME_FACTOR * cameraTransition.moveInTime) * Time.deltaTime);
            positionDifferenceMagnitude = (currentCameraFollowTransform.position - cameraTransition.targetTransform.position).magnitude;

            CameraScroll.Instance.LerpTowardsTargetDistance(cameraTransition.targetDistance, time / (cameraTransition.moveInTime) * 1 / (DISTANCE_CAMERA_TIME_FACTOR * cameraTransition.moveInTime));
            distanceDifferenceMagnitude = Math.Abs(CameraScroll.Instance.Distance - cameraTransition.targetDistance);

            time += Time.deltaTime;
            yield return null;
        }

        currentCameraFollowTransform.position = cameraTransition.targetTransform.position;

        OnCameraTransitionInEnd?.Invoke(this, new OnCameraTransitionEventArgs {cameraTransition = cameraTransition});

        if (!cameraTransition.endInTime) yield break;
        
        SetCameraState(State.LookingTarget);

        yield return new WaitForSeconds(cameraTransition.stallTime);

        yield return StartCoroutine(EndTransitionCoroutine(cameraTransition));
    }

    private IEnumerator EndTransitionCoroutine(CameraTransition cameraTransition)
    {
        if (state == State.MovingOut) yield break;
        if (state == State.FollowingPlayer) yield break;

        SetCameraState(State.MovingOut);

        OnCameraTransitionOutStart?.Invoke(this, new OnCameraTransitionEventArgs {cameraTransition = cameraTransition});

        float time = 0f;
        float positionDifferenceMagnitude = float.MaxValue;
        float distanceDifferenceMagnitude = float.MaxValue;

        while (positionDifferenceMagnitude > 0.2f)
        {
            currentCameraFollowTransform.position = Vector3.Lerp(currentCameraFollowTransform.position, playerCameraFollowPoint.position, time / (cameraTransition.moveOutTime) * 1 / (MOVE_CAMERA_TIME_FACTOR * cameraTransition.moveOutTime) * Time.deltaTime);
            positionDifferenceMagnitude = (currentCameraFollowTransform.position - playerCameraFollowPoint.position).magnitude;

            CameraScroll.Instance.LerpTowardsTargetDistance(previousCameraDistance, time / (cameraTransition.moveOutTime) * 1 / (DISTANCE_CAMERA_TIME_FACTOR * cameraTransition.moveOutTime));
            distanceDifferenceMagnitude = Math.Abs(CameraScroll.Instance.Distance - previousCameraDistance);

            time += Time.deltaTime;
            yield return null;
        }

        SetCameraState(State.StallingOut);

        time = 0f;

        while (time <= cameraTransition.stallTimeOut) //To rectify position during stallTimeOut
        {
            currentCameraFollowTransform.position = playerCameraFollowPoint.position;
            CameraScroll.Instance.LerpTowardsTargetDistance(previousCameraDistance, time / (cameraTransition.stallTimeOut) * 1 / (DISTANCE_CAMERA_TIME_FACTOR * cameraTransition.stallTimeOut));

            time += Time.deltaTime;
            yield return null;
        }

        currentCameraFollowTransform.position = playerCameraFollowPoint.position;
        CMVCam.Follow = playerCameraFollowPoint;

        Destroy(currentCameraFollowTransform.gameObject);
        ClearCurrentCameraFollowTransform();

        SetCameraState(State.FollowingPlayer);

        OnCameraTransitionOutEnd?.Invoke(this, new OnCameraTransitionEventArgs {cameraTransition = cameraTransition});
    }
}

[Serializable]
public class CameraTransition
{
    public int id;
    public string logToStart;
    public string logToEnd;
    public Transform targetTransform;
    [Range(0f, 4f)] public float stallTimeIn;
    [Range(0.5f, 4f)] public float moveInTime;
    [Range(0.5f, 10f)] public float stallTime;
    [Range(0.5f, 4f)] public float moveOutTime;
    [Range(0f, 4f)] public float stallTimeOut;
    [Range(2.5f, 8f)] public float targetDistance;
    public bool endInTime;
}