using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void TransitionMoveCamera(Transform targetTransform, float moveInTime, float watchTime, float moveOutTime)
    {
        if (state != State.FollowingPlayer) return;

        StopAllCoroutines();
        StartCoroutine(TransitionMoveCameraCoroutine(targetTransform, moveInTime, watchTime, moveOutTime));
    }

    private IEnumerator TransitionMoveCameraCoroutine(Transform targetTransform, float moveInTime, float watchTime, object moveOutTime)
    {
        SetCameraState(State.MovingTransition);

        float time = 0f;

        while (time <= moveInTime)
        {


            time += Time.deltaTime;
            yield return null;
        }

        SetCameraState(State.FollowingPlayer);
    }
}
