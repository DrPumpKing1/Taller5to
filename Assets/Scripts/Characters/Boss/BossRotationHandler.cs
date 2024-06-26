using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossRotationHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform transformToRotate;

    [Header("Starting Rotation")]
    [SerializeField] private Vector2 startingDirection;

    [Header("Settings")]
    [SerializeField, Range(1f, 100f)] private float smoothFollowPlayerRotateFactor;
    [SerializeField, Range(1f, 100f)] private float smoothTargetPlatformRotateFactor;

    [Header("States")]
    [SerializeField] private State rotationState;

    private enum State {NotRotating, FollowingPlayer, TargetingPlatform}

    private Vector3 desiredDirection;
    private Vector3 currentDirection;
    private float smoothRotateFactor;

    private Vector3 currentPlatformTargetedPosition;
    private GameObject player;

    private const string PLAYER_TAG = "Player";

    private void OnEnable()
    {
        BossStateHandler.OnBossActiveEnd += BossStateHandler_OnBossActiveEnd;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandler.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;

        BossObjectDestruction.OnProjectionPlatformTarget += BossObjectDestruction_OnProjectionPlatformTarget;
        BossObjectDestruction.OnProjectionPlatformTargetDestoyed += BossObjectDestruction_OnProjectionPlatformTargetDestoyed;
        BossObjectDestruction.OnProjectionPlatformTargetRemoved += BossObjectDestruction_OnProjectionPlatformTargetRemoved;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossActiveEnd -= BossStateHandler_OnBossActiveEnd;
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        BossStateHandler.OnPlayerDefeated -= BossStateHandler_OnPlayerDefeated;

        BossObjectDestruction.OnProjectionPlatformTarget -= BossObjectDestruction_OnProjectionPlatformTarget;
        BossObjectDestruction.OnProjectionPlatformTargetDestoyed -= BossObjectDestruction_OnProjectionPlatformTargetDestoyed;
        BossObjectDestruction.OnProjectionPlatformTargetRemoved -= BossObjectDestruction_OnProjectionPlatformTargetRemoved;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
    }

    private void Start()
    {
        InitializeVariables();
        InitializeRotation();
        SetRotationState(State.NotRotating);
    }

    private void Update()
    {
        HandleRotationState();
        HandleRotationSmooth();
    }

    private void InitializeVariables()
    {
        smoothRotateFactor = smoothFollowPlayerRotateFactor;
    }

    private void InitializeRotation()
    {
        Vector3 initialDirectionVector3 = GeneralMethods.Vector2ToVector3(startingDirection);
        initialDirectionVector3 = initialDirectionVector3.magnitude == 0 ? transformToRotate.forward : initialDirectionVector3;

        desiredDirection = initialDirectionVector3.normalized;
        currentDirection = desiredDirection;

        transformToRotate.localRotation = Quaternion.LookRotation(desiredDirection);
    }

    private void SetRotationState(State state) => rotationState = state;

    private void HandleRotationState()
    {
        switch (rotationState)
        {
            case (State.NotRotating):
                NotRotatingLogic();
                break;
            case (State.FollowingPlayer):
                FollowingPlayerLogic();
                break;
            case (State.TargetingPlatform):
                TargetingPlatformLogic();
                break;
        }
    }

    private void NotRotatingLogic() { }

    private void FollowingPlayerLogic()
    {
        smoothRotateFactor = smoothFollowPlayerRotateFactor;

        Vector3 playerDirection = (transform.position - player.transform.position).normalized;

        desiredDirection = GeneralMethods.SupressYComponent(playerDirection);
    }

    private void TargetingPlatformLogic()
    {
        smoothRotateFactor = smoothTargetPlatformRotateFactor;

        Vector3 platformDirection = (transform.position - currentPlatformTargetedPosition).normalized;

        desiredDirection = GeneralMethods.SupressYComponent(platformDirection);
    }

    private void HandleRotationSmooth()
    {
        currentDirection = Vector3.Slerp(currentDirection, desiredDirection, smoothRotateFactor * Time.deltaTime);
        currentDirection.Normalize();

        transformToRotate.localRotation = Quaternion.LookRotation(currentDirection);
    }

    private void SetCurrentTargetetPlatformPosition(Vector3 position) => currentPlatformTargetedPosition = position;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e)
    {
        SetRotationState(State.FollowingPlayer);
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        SetRotationState(State.NotRotating);
    }

    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        SetRotationState(State.FollowingPlayer);
    }
    private void BossObjectDestruction_OnProjectionPlatformTarget(object sender, BossObjectDestruction.OnProjectionPlatformTargetEventArgs e)
    {
        SetCurrentTargetetPlatformPosition(e.projectionPlatform.transform.position);
        SetRotationState(State.TargetingPlatform);
    }

    private void BossObjectDestruction_OnProjectionPlatformTargetDestoyed(object sender, BossObjectDestruction.OnProjectionPlatformTargetEventArgs e)
    {
        SetRotationState(State.FollowingPlayer);
    }

    private void BossObjectDestruction_OnProjectionPlatformTargetRemoved(object sender, BossObjectDestruction.OnProjectionPlatformTargetEventArgs e)
    {
        if (rotationState == State.NotRotating) return;
        SetRotationState(State.FollowingPlayer);
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        SetRotationState(State.NotRotating);

    }
    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        SetRotationState(State.NotRotating);
    }
    #endregion
}
