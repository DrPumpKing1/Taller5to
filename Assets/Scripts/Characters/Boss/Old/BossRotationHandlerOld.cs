using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossRotationHandlerOld : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform transformToRotate;

    [Header("Starting Rotation")]
    [SerializeField] private Vector2 startingDirection;

    [Header("Settings")]
    [SerializeField, Range(0.5f, 2f)] private float timeRotationTargeting;
    [SerializeField, Range(1f, 100f)] private float smoothFollowPlayerRotateFactor;
    [SerializeField, Range(1f, 100f)] private float smoothTargetRotateFactor;

    [Header("States")]
    [SerializeField] private State rotationState;

    private enum State {NotRotating, FollowingPlayer, Targeting}

    private Vector3 desiredDirection;
    private Vector3 currentDirection;
    private float smoothRotateFactor;

    private Vector3 currentTargetedPosition;
    private GameObject player;

    private const string PLAYER_TAG = "Player";

    private float timer;

    private void OnEnable()
    {
        BossStateHandlerOld.OnBossActiveEnd += BossStateHandler_OnBossActiveEnd;
        BossStateHandlerOld.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;
        BossStateHandlerOld.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandlerOld.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;

        BossObjectDestructionOld.OnProjectionPlatformTarget += BossObjectDestruction_OnProjectionPlatformTarget;
        BossSourceStunOld.OnSourceStunned += BossSourceStun_OnSourceStunned;
    }

    private void OnDisable()
    {
        BossStateHandlerOld.OnBossActiveEnd -= BossStateHandler_OnBossActiveEnd;
        BossStateHandlerOld.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;
        BossStateHandlerOld.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        BossStateHandlerOld.OnPlayerDefeated -= BossStateHandler_OnPlayerDefeated;

        BossObjectDestructionOld.OnProjectionPlatformTarget -= BossObjectDestruction_OnProjectionPlatformTarget;
        BossSourceStunOld.OnSourceStunned -= BossSourceStun_OnSourceStunned;
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
        timer = 0f;
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
            case (State.Targeting):
                TargetingLogic();
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

    private void TargetingLogic()
    {
        smoothRotateFactor = smoothTargetRotateFactor;

        Vector3 targetDirection = (transform.position - currentTargetedPosition).normalized;

        desiredDirection = GeneralMethods.SupressYComponent(targetDirection);

        if (timer >= timeRotationTargeting)
        {
            SetRotationState(State.FollowingPlayer);
            ResetTimer();
            return;
        }

        timer += Time.deltaTime;
    }

    private void HandleRotationSmooth()
    {
        currentDirection = Vector3.Slerp(currentDirection, desiredDirection, smoothRotateFactor * Time.deltaTime);
        currentDirection.Normalize();

        transformToRotate.localRotation = Quaternion.LookRotation(currentDirection);
    }

    private void SetCurrentTargetPosition(Vector3 position) => currentTargetedPosition = position;

    private void ResetTimer() => timer = 0f;

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e)
    {
        SetRotationState(State.FollowingPlayer);
        ResetTimer();
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandlerOld.OnPhaseChangeEventArgs e)
    {
        SetRotationState(State.NotRotating);
        ResetTimer();
    }

    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandlerOld.OnPhaseChangeEventArgs e)
    {
        SetRotationState(State.FollowingPlayer);
        ResetTimer();
    }
    private void BossObjectDestruction_OnProjectionPlatformTarget(object sender, BossObjectDestructionOld.OnProjectionPlatformTargetEventArgs e)
    {
        SetCurrentTargetPosition(e.projectionPlatform.transform.position);
        SetRotationState(State.Targeting);
        ResetTimer();
    }

    private void BossSourceStun_OnSourceStunned(object sender, BossSourceStunOld.OnSourceStunnedEventArgs e)
    {
        SetCurrentTargetPosition(e.stuneableSource.transform.position);
        SetRotationState(State.Targeting);
        ResetTimer();
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        SetRotationState(State.NotRotating);
        ResetTimer();
    }
    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        SetRotationState(State.NotRotating);
        ResetTimer();
    }
    #endregion
}
