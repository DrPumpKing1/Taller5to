using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetStateHandler : MonoBehaviour
{
    public static PetStateHandler Instance {  get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    public enum State { Still, FollowingPlayer, OnGuidance }
    public State PetState => state;

    public PetGuidanceListener.PetGuidanceObject CurrentPetGuidanceObject { get; private set; }

    private void OnEnable()
    {
        PetPlayerAttachment.OnVyrxAttachToPlayer += PetPlayerAttachment_OnVyrxAttachToPlayer;
        PetPlayerAttachment.OnVyrxUnattachToPlayer += PetPlayerAttachment_OnVyrxUnattachToPlayer;

        PetGuidanceListener.OnPetGuidanceStart += PetGuidanceListener_OnPetGuidanceStart;
        PetGuidanceListener.OnPetGuidanceEnd += PetGuidanceListener_OnPetGuidanceEnd;
    }

    private void OnDisable()
    {
        PetPlayerAttachment.OnVyrxAttachToPlayer -= PetPlayerAttachment_OnVyrxAttachToPlayer;
        PetPlayerAttachment.OnVyrxUnattachToPlayer -= PetPlayerAttachment_OnVyrxUnattachToPlayer;

        PetGuidanceListener.OnPetGuidanceStart -= PetGuidanceListener_OnPetGuidanceStart;
        PetGuidanceListener.OnPetGuidanceEnd -= PetGuidanceListener_OnPetGuidanceEnd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        ClearPetGuidanceObject();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one PetPlayerAttachment instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetPetState(State state) => this.state = state;

    #region GuidanceMethods
    private bool CanStartGuidance() => state != State.Still;

    private void SetGuidanceObject(PetGuidanceListener.PetGuidanceObject petGuidanceObject) => CurrentPetGuidanceObject = petGuidanceObject;
    private void ClearPetGuidanceObject() => CurrentPetGuidanceObject = null;
    #endregion

    #region PetGuidanceListener Subscriptions
    private void PetGuidanceListener_OnPetGuidanceStart(object sender, PetGuidanceListener.OnPetGuidanceEventArgs e)
    {
        if (!CanStartGuidance()) return;

        SetGuidanceObject(e.petGuidanceObject);
        SetPetState(State.OnGuidance);
    }

    private void PetGuidanceListener_OnPetGuidanceEnd(object sender, PetGuidanceListener.OnPetGuidanceEventArgs e)
    {
        if (!CanStartGuidance()) return;

        ClearPetGuidanceObject();
        SetPetState(State.FollowingPlayer);
    }
    #endregion

    #region PetPlayerAttachment Subscriptions
    private void PetPlayerAttachment_OnVyrxAttachToPlayer(object sender, EventArgs e)
    {
        SetPetState(State.FollowingPlayer);
    }

    private void PetPlayerAttachment_OnVyrxUnattachToPlayer(object sender, EventArgs e)
    {
        SetPetState(State.Still);
    }

    #endregion
}
