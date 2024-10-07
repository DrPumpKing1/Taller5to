using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Resources;
using System.Threading;

public class StunableProjectionPlatformProjection : ProjectionPlatformProjection
{
    [Header("Stun Settings")]
    [SerializeField] private bool isStunned;

    [Header("States")]
    [SerializeField] private State state;

    public enum State { NotStunned, Stunned}

    public State PlatformState => state;

    private float timer;

    public event EventHandler OnProjectionPlatformStun;
    public event EventHandler OnProjectionPlatformEndStun;

    public event EventHandler OnProjectionPlatformFailInteractStun;

    protected override void OnEnable()
    {
        base.OnEnable();
        BossBeam.OnBeamPlatformStun += BossBeam_OnBeamPlatformStun;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        BossBeam.OnBeamPlatformStun -= BossBeam_OnBeamPlatformStun;
    }

    private void Update()
    {
        HandlePlatformStunState();
    }

    private void HandlePlatformStunState()
    {
        switch (state)
        {
            case State.NotStunned:
                NotStunnedLogic();
                break;
            case State.Stunned:
                StunnedLogic();
                break;
        }
    }

    private void NotStunnedLogic()
    {
        ResetTimer();
    }

    private void StunnedLogic()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            return;
        }

        ResetTimer();
        EndProjectionPlatformStun();
        SetPlatformState(State.NotStunned); 
    }

    private void SetPlatformState(State state) => this.state = state;

    public void StunProjectionPlatform(float stunTime)
    {
        if(state != State.NotStunned) return;

        isInteractable = false;
        SetPlatformState(State.Stunned);
        SetTimer(stunTime);

        OnProjectionPlatformStun?.Invoke(this, EventArgs.Empty);
    }

    public void EndProjectionPlatformStun()
    {
        if (state != State.Stunned) return;

        isInteractable = true;
        SetPlatformState(State.NotStunned);
        ResetTimer();

        OnProjectionPlatformEndStun?.Invoke(this, EventArgs.Empty);
    }

    private void SetTimer(float time) => timer = time;
    private void ResetTimer() => timer = 0f;

    public override void FailInteract()
    {
        base.FailInteract();

        if (state != State.Stunned) return;
        OnProjectionPlatformFailInteractStun?.Invoke(this, EventArgs.Empty);
        Debug.Log("Stunned!");
    }


    #region BossBeam Subscriptions
    private void BossBeam_OnBeamPlatformStun(object sender, BossBeam.OnBeamPlatformStunEventArgs e)
    {
        if (e.stunableProjectionPlatformProjection != this) return;
        if (state != State.NotStunned) return;

        StunProjectionPlatform(e.stunTime);
    }
    #endregion
}
