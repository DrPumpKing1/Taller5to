using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static PetGuidanceListener;

public class PetGuidanceListener : MonoBehaviour
{
    [Header("Guidance Settings")]
    [SerializeField] private List<PetGuidance> petGuidances;

    [Serializable]
    public class PetGuidance
    {
        public string logToStartGuidance;
        public string logToEndGuidance;
        public Transform positionTransform;
        public Transform lookTransform;
    }

    public static event EventHandler<OnPetGuidanceEventArgs> OnPetGuidanceStart;
    public static event EventHandler<OnPetGuidanceEventArgs> OnPetGuidanceEnd;

    public class OnPetGuidanceEventArgs : EventArgs
    {
        public Transform positionTransform;
        public Transform lookTransform;
    }

    private PetGuidance currentGuidance;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Start()
    {
        ClearCurrentGuidance();
    }

    private void SetCurrentGuidance(PetGuidance petGuidance) => currentGuidance = petGuidance;
    private void ClearCurrentGuidance() => currentGuidance = null;

    private void CheckStartGuidance(string log)
    {
        foreach(PetGuidance petGuidance in petGuidances)
        {
            if(petGuidance.logToStartGuidance == log)
            {
                SetCurrentGuidance(petGuidance);
                OnPetGuidanceStart?.Invoke(this, new OnPetGuidanceEventArgs { positionTransform = petGuidance.positionTransform, lookTransform = petGuidance.lookTransform });
                return;
            }
        }
    }

    private void CheckEndGuidance(string log)
    {
        if (currentGuidance == null) return;
        if (currentGuidance.logToEndGuidance != log) return;

        OnPetGuidanceEnd?.Invoke(this, new OnPetGuidanceEventArgs { positionTransform = currentGuidance.positionTransform, lookTransform = currentGuidance.lookTransform });
        ClearCurrentGuidance();
        return;
    }

    #region GameLogManagerSubscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckEndGuidance(e.gameplayAction.log);
        CheckStartGuidance(e.gameplayAction.log);
    }
    #endregion
}
