using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomHandler : MonoBehaviour
{
    public static CameraZoomHandler Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;

    public enum State { ListeningPlayer, StallingIn, ZoomingIn, LookingTarget, ZoomingOut, StallingOut }
    public State CameraState => state;

    private const float DISTANCE_CAMERA_TIME_FACTOR = 0.08f;

    private float previousCameraDistance;

    public static event EventHandler<OnCameraZoomEventArgs> OnCameraZoomInStart;
    public static event EventHandler<OnCameraZoomEventArgs> OnCameraZoomInEnd;
    public static event EventHandler<OnCameraZoomEventArgs> OnCameraZoomOutStart;
    public static event EventHandler<OnCameraZoomEventArgs> OnCameraZoomOutEnd;

    public class OnCameraZoomEventArgs : EventArgs
    {
        public CameraZoom cameraZoom;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetCameraState(State.ListeningPlayer);
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

    private void SetCameraState(State state) => this.state = state;
    private void SetPreviousCameraDistance(float distance) => previousCameraDistance = distance;
    public bool AllowCameraInputProcessing() => state == State.ListeningPlayer || state == State.StallingOut;
    public bool AllowMovementInputProcessing() => state == State.ListeningPlayer || state == State.StallingOut;

    private bool CanStartZoom() => state == State.ListeningPlayer || state == State.ZoomingOut || state == State.StallingOut;
    private bool WasListeningToPlayer() => state == State.ListeningPlayer;
    private bool CanEndZoom() => state != State.ZoomingOut && state != State.ListeningPlayer;

    public void ZoomCamera(CameraZoom cameraZoom)
    {
        if (CameraTransitionHandler.Instance.CameraState != CameraTransitionHandler.State.FollowingPlayer) return;
        if (!CanStartZoom()) return;


        if(WasListeningToPlayer()) SetPreviousCameraDistance(CameraScroll.Instance.Distance);

        StopAllCoroutines();
        StartCoroutine(ZoomCameraCoroutine(cameraZoom));
    }

    public void EndZoom(CameraZoom cameraZoom)
    {
        if (!CanEndZoom()) return;

        StopAllCoroutines();
        StartCoroutine(EndZoomCoroutine(cameraZoom));
    }

    public void CancelZoom()
    {
        if (state == State.ListeningPlayer) return;

        StopAllCoroutines();
        SetCameraState(State.ListeningPlayer);
    }

    private IEnumerator ZoomCameraCoroutine(CameraZoom cameraZoom)
    {
        OnCameraZoomInStart?.Invoke(this, new OnCameraZoomEventArgs { cameraZoom = cameraZoom });

        SetCameraState(State.StallingIn);

        yield return new WaitForSeconds(cameraZoom.stallTimeIn);

        SetCameraState(State.ZoomingIn);

        float time = 0f;
        float distanceDifferenceMagnitude = float.MaxValue;

        while (distanceDifferenceMagnitude > 0.1f)
        {
            CameraScroll.Instance.LerpTowardsTargetDistance(cameraZoom.targetDistance, time / (cameraZoom.zoomInTime) * 1 / (DISTANCE_CAMERA_TIME_FACTOR * cameraZoom.zoomInTime));
            distanceDifferenceMagnitude = Math.Abs(CameraScroll.Instance.Distance - cameraZoom.targetDistance);

            time += Time.deltaTime;
            yield return null;
        }

        OnCameraZoomInEnd?.Invoke(this, new OnCameraZoomEventArgs { cameraZoom = cameraZoom });

        if (!cameraZoom.endInTime) yield break;

        SetCameraState(State.LookingTarget);

        yield return new WaitForSeconds(cameraZoom.stallTime);

        yield return StartCoroutine(EndZoomCoroutine(cameraZoom));
    }

    private IEnumerator EndZoomCoroutine(CameraZoom cameraZoom)
    {
        if (state == State.ZoomingOut) yield break;
        if (state == State.ListeningPlayer) yield break;

        SetCameraState(State.ZoomingOut);

        OnCameraZoomOutStart?.Invoke(this, new OnCameraZoomEventArgs { cameraZoom = cameraZoom });

        float time = 0f;
        float distanceDifferenceMagnitude = float.MaxValue;

        while (distanceDifferenceMagnitude > 0.1f)
        {
            CameraScroll.Instance.LerpTowardsTargetDistance(previousCameraDistance, time / (cameraZoom.zoomOutTime) * 1 / (DISTANCE_CAMERA_TIME_FACTOR * cameraZoom.zoomOutTime));
            distanceDifferenceMagnitude = Math.Abs(CameraScroll.Instance.Distance - previousCameraDistance);

            time += Time.deltaTime;
            yield return null;
        }

        SetCameraState(State.StallingOut);

        time = 0f;

        while (time <= cameraZoom.stallTimeOut) //To rectify zoom during stallTimeOut
        {
            CameraScroll.Instance.LerpTowardsTargetDistance(previousCameraDistance, time / (cameraZoom.stallTimeOut) * 1 / (DISTANCE_CAMERA_TIME_FACTOR * cameraZoom.stallTimeOut));

            time += Time.deltaTime;
            yield return null;
        }

        SetCameraState(State.ListeningPlayer);

        OnCameraZoomOutEnd?.Invoke(this, new OnCameraZoomEventArgs { cameraZoom = cameraZoom });
    }
}

[Serializable]
public class CameraZoom
{
    public int id;
    public string logToStart;
    public string logToEnd;
    [Range(0f, 4f)] public float stallTimeIn;
    [Range(0.5f, 4f)] public float zoomInTime;
    [Range(0.5f, 10f)] public float stallTime;
    [Range(0.5f, 4f)] public float zoomOutTime;
    [Range(0.01f, 4f)] public float stallTimeOut;
    [Range(2.5f, 8f)] public float targetDistance;
    public bool endInTime;
}
