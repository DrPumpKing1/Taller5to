using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowcaseRoomSurge : MonoBehaviour
{
    public static ShowcaseRoomSurge Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private List<PhaseActiveLayout> phaseActiveLayouts;

    public static event EventHandler<OnShowcaseRoomSurgeEventArgs> OnShowcaseRoomSurge;

    private PhaseActiveLayout currentActiveLayout;

    private const ShowcaseRoomPhase STARTING_SURGE_PHASE = ShowcaseRoomPhase.Phase1;

    public class OnShowcaseRoomSurgeEventArgs : EventArgs
    {
        public PhaseActiveLayout phaseActiveLayout;
    }

    [Serializable]
    public class PhaseActiveLayout
    {
        public ShowcaseRoomPhase activePhase;
        public Transform layoutTransform;
        public Transform activeLayoutPosition;
        public Transform disabledLayoutPosition;
    }

    private void OnEnable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidB += ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidB;
    }

    private void OnDisable()
    {
        ShowcaseRoomStateHandler.OnShowcaseRoomPhaseChangeMidB -= ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidB;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        ClearCurrentActiveLayout();
        CheckSurge(STARTING_SURGE_PHASE, false);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ShowcaseRoomSurge, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
    private void CheckSurge(ShowcaseRoomPhase phase, bool triggerEvents)
    {
        foreach (PhaseActiveLayout phaseActiveLayout in phaseActiveLayouts)
        {
            if (phaseActiveLayout.activePhase == phase)
            {
                Surge(phaseActiveLayout, triggerEvents);
                return;
            }
        }
    }

    private void Surge(PhaseActiveLayout targetPhaseActiveLayout, bool triggerEvents)
    {
        foreach (PhaseActiveLayout phaseActiveLayout in phaseActiveLayouts) //Move all nonActives underground;
        {
            if (phaseActiveLayout != targetPhaseActiveLayout) SetLayout(phaseActiveLayout, false);
        }

        SetCurrentActiveLayout(targetPhaseActiveLayout);
        SetLayout(targetPhaseActiveLayout, true); //Move Active layout up
        if (triggerEvents) OnShowcaseRoomSurge?.Invoke(this, new OnShowcaseRoomSurgeEventArgs { phaseActiveLayout = targetPhaseActiveLayout });
    }

    private void SetLayout(PhaseActiveLayout phaseActiveLayout, bool active)
    {
        if (active) phaseActiveLayout.layoutTransform.position = phaseActiveLayout.activeLayoutPosition.position;
        else phaseActiveLayout.layoutTransform.position = phaseActiveLayout.disabledLayoutPosition.position;
    }

    private void SetCurrentActiveLayout(PhaseActiveLayout phaseActiveLayout) => currentActiveLayout = phaseActiveLayout;
    private void ClearCurrentActiveLayout() => currentActiveLayout = null;

    #region ShowcaseRoomStateHandler Subscriptions
    private void ShowcaseRoomStateHandler_OnShowcaseRoomPhaseChangeMidB(object sender, ShowcaseRoomStateHandler.OnPhaseChangeEventArgs e)
    {
        CheckSurge(e.nextPhase, true);
    }
    #endregion
}
