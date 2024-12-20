using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BossShieldOld : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform shieldHolder;
    [SerializeField] private Transform shield;

    [Header("Settings")]
    [SerializeField] private bool enableSinceFirstPhase;
    [SerializeField] private int phaseNumberToEnable;
    [SerializeField, Range(1f, 100f)] private float shieldSmoothRotateFactor;

    [Header("Starting Rotation")]
    [SerializeField] private Vector2 startingDirection;

    [Header("States")]
    [SerializeField] private State bossShieldState;

    [Header("Debug")]
    [SerializeField] private BossAttackDirectionHandlerOld.AttackDirection currentShieldDirection = null;

    private Vector3 desiredDirection;
    private Vector3 currentDirection;

    private enum State { Disabled, Shielding }

    private void OnEnable()
    {
        BossStateHandlerOld.OnBossActiveStart += BossStateHandler_OnBossActiveStart;
        BossStateHandlerOld.OnBossActiveEnd += BossStateHandler_OnBossActiveEnd;

        BossStateHandlerOld.OnBossPhaseChangeStart += BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossPhaseChangeEnd += BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandlerOld.OnBossDefeated += BossStateHandler_OnBossDefeated;
        BossStateHandlerOld.OnPlayerDefeated += BossStateHandler_OnPlayerDefeated;
    }


    private void OnDisable()
    {
        BossStateHandlerOld.OnBossActiveStart -= BossStateHandler_OnBossActiveStart;
        BossStateHandlerOld.OnBossActiveEnd -= BossStateHandler_OnBossActiveEnd;

        BossStateHandlerOld.OnBossPhaseChangeStart -= BossStateHandler_OnBossPhaseChangeStart;
        BossStateHandlerOld.OnBossPhaseChangeEnd -= BossStateHandler_OnBossPhaseChangeEnd;

        BossStateHandlerOld.OnBossDefeated -= BossStateHandler_OnBossDefeated;
        BossStateHandlerOld.OnPlayerDefeated -= BossStateHandler_OnPlayerDefeated;
    }

    private void Start()
    {
        InitializeShieldRotation();
        DisableShield();
    }

    private void Update()
    {
        HandleBossShieldState();
        HandleShieldRotationSmooth();
    }

    private void InitializeShieldRotation()
    {
        Vector3 initialDirectionVector3 = GeneralMethods.Vector2ToVector3(startingDirection);
        initialDirectionVector3 = initialDirectionVector3.magnitude == 0 ? shieldHolder.forward : initialDirectionVector3;

        desiredDirection = initialDirectionVector3.normalized;
        currentDirection = desiredDirection;

        shieldHolder.localRotation = Quaternion.LookRotation(desiredDirection);
    }

    private void SetBossShieldState(State state) => bossShieldState = state;

    private void HandleBossShieldState()
    {
        switch (bossShieldState)
        {
            case State.Disabled:
                DisabledLogic();
                break;
            case State.Shielding:
                ShieldingLogic();
                break;
        }
    }

    private void DisabledLogic() { }
    private void ShieldingLogic()
    {
        List<BossAttackDirectionHandlerOld.AttackDirection> currentAttackDirections = BossAttackDirectionHandlerOld.Instance.CurrentAttackDirections;

        if (currentAttackDirections.Count == 0) return;

        if (!currentAttackDirections.Contains(currentShieldDirection))
        {
            currentShieldDirection = currentAttackDirections[0];
            desiredDirection = GeneralMethods.Vector2ToVector3(currentShieldDirection.vectorizedDirection);
        }
    }

    private void HandleShieldRotationSmooth()
    {
        currentDirection = Vector3.Slerp(currentDirection, desiredDirection, shieldSmoothRotateFactor * Time.deltaTime);
        currentDirection.Normalize();

        shieldHolder.localRotation = Quaternion.LookRotation(currentDirection);
    }

    private void EnableShield() => shield.gameObject.SetActive(true);
    private void DisableShield() => shield.gameObject.SetActive(false);

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossActiveStart(object sender, EventArgs e)
    {
        SetBossShieldState(State.Disabled);

        if (enableSinceFirstPhase)
        {
            EnableShield();
        }
        else
        {
            DisableShield();
        }
    }
    private void BossStateHandler_OnBossActiveEnd(object sender, EventArgs e)
    {
        if (enableSinceFirstPhase)
        {
            SetBossShieldState(State.Shielding);
            EnableShield();
        }
    }

    private void BossStateHandler_OnBossPhaseChangeStart(object sender, BossStateHandlerOld.OnPhaseChangeEventArgs e)
    {
        SetBossShieldState(State.Disabled);

        if (e.phaseNumber >= phaseNumberToEnable)
        {
            EnableShield();
        }
    }

    private void BossStateHandler_OnBossPhaseChangeEnd(object sender, BossStateHandlerOld.OnPhaseChangeEventArgs e)
    {
        if (e.phaseNumber >= phaseNumberToEnable)
        {
            SetBossShieldState(State.Shielding);
        }
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        SetBossShieldState(State.Disabled);
        DisableShield();
    }
    private void BossStateHandler_OnPlayerDefeated(object sender, EventArgs e)
    {
        SetBossShieldState(State.Disabled);
    }
    #endregion
}
