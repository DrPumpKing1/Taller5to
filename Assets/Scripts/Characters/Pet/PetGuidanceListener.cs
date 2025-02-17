using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PetGuidanceListener : MonoBehaviour
{
    [Header("Guidance Settings")]
    [SerializeField] private List<PetGuidanceObject> petGuidanceObjects;

    [Serializable]
    public class PetGuidanceObject
    {
        public int id;
        public string guidanceName;
        public string logToStartGuidance;
        public string logToEndGuidance;
        public Transform positionTransform;
        public Transform lookTransform;
    }

    public static event EventHandler<OnPetGuidanceEventArgs> OnPetGuidanceStart;
    public static event EventHandler<OnPetGuidanceEventArgs> OnPetGuidanceEnd;

    public class OnPetGuidanceEventArgs : EventArgs
    {
        public PetGuidanceObject petGuidanceObject;
    }

    private PetGuidanceObject currentPetGuidanceObject;

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

    private void SetCurrentPetGuidanceObject(PetGuidanceObject petGuidance) => currentPetGuidanceObject = petGuidance;
    private void ClearCurrentGuidance() => currentPetGuidanceObject = null;

    private void CheckStartGuidance(string log)
    {
        foreach(PetGuidanceObject petGuidanceObject in petGuidanceObjects)
        {
            if(petGuidanceObject.logToStartGuidance == log)
            {
                StartGuidance(petGuidanceObject);
                return;
            }
        }
    }

    private void CheckEndGuidance(string log)
    {
        if (currentPetGuidanceObject == null) return;
        if (currentPetGuidanceObject.logToEndGuidance != log) return;

        EndGuidance();
        return;
    }

    private void StartGuidance(PetGuidanceObject petGuidanceObject)
    {
        SetCurrentPetGuidanceObject(petGuidanceObject);
        OnPetGuidanceStart?.Invoke(this, new OnPetGuidanceEventArgs { petGuidanceObject = petGuidanceObject });
    }

    private void EndGuidance()
    {
        OnPetGuidanceEnd?.Invoke(this, new OnPetGuidanceEventArgs { petGuidanceObject = currentPetGuidanceObject });
        ClearCurrentGuidance();
    }

    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckEndGuidance(e.gameplayAction.log);
        CheckStartGuidance(e.gameplayAction.log);
    }
    #endregion
}
