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
    [SerializeField] private float timeToRotate;
    [SerializeField] private bool clockwiseRotation;
    [SerializeField] private int degreesPerTurn;
    [SerializeField, Range(1f, 100f)] private float smoothRotateFactor;
    [SerializeField] private bool resetTimerOnPhaseChange;
    [Space]
    [SerializeField] private float timer;

    private Vector3 desiredDirection;
    private Vector3 currentDirection;

    public static event EventHandler<OnBossRotatedEventArgs> OnBossRotated;

    public class OnBossRotatedEventArgs : EventArgs
    {

    }
    private void OnEnable()
    {
        BossStateHandler.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandler.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;
    }


    private void OnDisable()
    {
        BossStateHandler.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandler.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandler.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        BossStateHandler.OnPlayerDefeated -= BossStateHandler_OnPlayerDefeated;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleRotation();
        HandleRotationSmooth();
    }
    private void InitializeVariables()
    {
        ResetTimer();
        InitializeRotation();
    }

    private void InitializeRotation()
    {
        Vector3 initialDirectionVector3 = GeneralMethods.Vector2ToVector3(startingDirection);
        initialDirectionVector3 = initialDirectionVector3.magnitude == 0 ? transformToRotate.forward : initialDirectionVector3;

        desiredDirection = initialDirectionVector3.normalized;
        currentDirection = desiredDirection;

        transformToRotate.localRotation = Quaternion.LookRotation(desiredDirection);
    }


    private void HandleRotation()
    {
        if (BossStateHandler.Instance.BossState != BossStateHandler.State.OnPhase) return;

        if (timer > 0) timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Rotate();
            ResetTimer();
        }
    }

    private void HandleRotationSmooth()
    {
        currentDirection = Vector3.Slerp(currentDirection, desiredDirection, smoothRotateFactor * Time.deltaTime);
        currentDirection.Normalize();

        transformToRotate.localRotation = Quaternion.LookRotation(currentDirection);
    }

    private void Rotate()
    {
        float degreesToTurn = clockwiseRotation ? degreesPerTurn : -degreesPerTurn;
        desiredDirection = (Quaternion.AngleAxis(degreesToTurn, Vector3.up) * desiredDirection).normalized;

        OnBossRotated?.Invoke(this, new OnBossRotatedEventArgs { });
    }

    private void ResetTimer() => timer = timeToRotate;

    #region BossStateHandler Subscriptions

    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e)
    {
        if (!resetTimerOnPhaseChange) return;
        ResetTimer();
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, EventArgs e)
    {
        if (!resetTimerOnPhaseChange) return;
        ResetTimer();
    }
    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        ResetTimer();
    }
    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        ResetTimer();
    }
    #endregion
}
