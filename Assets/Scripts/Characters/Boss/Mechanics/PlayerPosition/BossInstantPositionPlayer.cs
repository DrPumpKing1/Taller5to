using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class BossInstantPositionPlayer : MonoBehaviour
{
    public static BossInstantPositionPlayer Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private List<PhasePlayerPosition> phasePlayerPositions;

    [Serializable]
    private class PhasePlayerPosition
    {
        public BossPhase bossPhase;
        public Transform positionTransform;
        public Vector2 rotationDirection;
    }

    private void OnEnable()
    {
        BossStateHandler.OnBossPhaseChangeMidA += BossStateHandler_OnBossPhaseChangeMidA;
    }

    private void OnDisable()
    {
        BossStateHandler.OnBossPhaseChangeMidA -= BossStateHandler_OnBossPhaseChangeMidA;
    }

    private void CheckPlayerInstantPosition(BossPhase bossPhase)
    {
        foreach(PhasePlayerPosition phasePlayerPosition in phasePlayerPositions)
        {
            if(phasePlayerPosition.bossPhase == bossPhase)
            {
                InstantPositionPlayer(phasePlayerPosition);
                return;
            }
        }
    }

    private void InstantPositionPlayer(PhasePlayerPosition phasePlayerPosition)
    {
        PlayerPositioningHandler.Instance.InstantPositionPlayer(phasePlayerPosition.positionTransform.position);
        PlayerDirectionHandler.Instance.InstantDirectionPlayer(phasePlayerPosition.rotationDirection);
        PlayerPositioningHandler.Instance.gameObject.transform.SetParent(null);
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one BossInstantPositionPlayer, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region BossStateHandler Subscriptions
    private void BossStateHandler_OnBossPhaseChangeMidA(object sender, BossStateHandler.OnPhaseChangeEventArgs e)
    {
        CheckPlayerInstantPosition(e.nextPhase);
    }

    #endregion
}
