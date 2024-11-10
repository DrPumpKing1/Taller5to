using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomInstantPositionPlayer : MonoBehaviour
{
    public static ShowcaseRoomInstantPositionPlayer Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private List<PhasePlayerPosition> phasePlayerPositions;

    [Serializable]
    private class PhasePlayerPosition
    {
        public ShowcaseRoomPhase showcaseRoomPhase;
        public Transform positionTransform;
        public Vector2 rotationDirection;
    }

    private void OnEnable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidA += ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidA;
    }

    private void OnDisable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidA -= ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidA;
    }

    private void CheckPlayerInstantPosition(ShowcaseRoomPhase bossPhase)
    {
        foreach (PhasePlayerPosition phasePlayerPosition in phasePlayerPositions)
        {
            if (phasePlayerPosition.showcaseRoomPhase == bossPhase)
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
            Debug.LogWarning("There is more than one ShowcaseRoomInstantPositionPlayer, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region BossStateHandler Subscriptions
    private void ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidA(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        CheckPlayerInstantPosition(e.nextPhase);
    }

    #endregion
}
