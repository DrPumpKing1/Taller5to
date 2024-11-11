using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ShowcaseRoomWeakpoint : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;

    [Header("Settings")]
    [SerializeField] private bool isEnabled;
    [SerializeField] private bool isHit;

    public int ID => id;
    public bool IsEnabled => isEnabled;
    public bool IsHit => isHit;

    private GameObject player;

    private const string PLAYER_TAG = "Player";
    private const float PLAYER_DISTANCE_TO_HIT = 100f;

    public static event EventHandler<OnShowcaseRoomWeakPointEventArgs> OnShowcaseRoomWeakpointEnable;
    public static event EventHandler<OnShowcaseRoomWeakPointEventArgs> OnShowcaseRoomWeakpointDisable;

    public event EventHandler OnWeakPointEnableA;
    public event EventHandler OnWeakPointEnableB;
    public event EventHandler OnWeakPointDisableA;
    public event EventHandler OnWeakPointDisableB;

    public static event EventHandler OnAnyWeakpointUnHit;
    public static event EventHandler OnAnyWeakpointHit;
    public event EventHandler OnWeakpointHit;
    public event EventHandler OnWeakpointUnHit;

    private void OnEnable()
    {
        ShowcaseRoomWeakPointsHandler.OnWeakPointsEnableA += ShowcaseRoomsWeakPointsHandler_OnWeakPointsEnableA;
        ShowcaseRoomWeakPointsHandler.OnWeakPointsEnableB += ShowcaseRoomsWeakPointsHandler_OnWeakPointsEnableB;
        ShowcaseRoomWeakPointsHandler.OnWeakPointsDisableA += ShowcaseRoomsWeakPointsHandler_OnWeakPointsDisableA;
        ShowcaseRoomWeakPointsHandler.OnWeakPointsDisableB += ShowcaseRoomsWeakPointsHandler_OnWeakPointsDisableB;
    }

    private void OnDisable()
    {
        ShowcaseRoomWeakPointsHandler.OnWeakPointsEnableA -= ShowcaseRoomsWeakPointsHandler_OnWeakPointsEnableA;
        ShowcaseRoomWeakPointsHandler.OnWeakPointsEnableB -= ShowcaseRoomsWeakPointsHandler_OnWeakPointsEnableB;
        ShowcaseRoomWeakPointsHandler.OnWeakPointsDisableA -= ShowcaseRoomsWeakPointsHandler_OnWeakPointsDisableA;
        ShowcaseRoomWeakPointsHandler.OnWeakPointsDisableB -= ShowcaseRoomsWeakPointsHandler_OnWeakPointsDisableB;
    }

    public class OnShowcaseRoomWeakPointEventArgs : EventArgs
    {
        public ShowcaseRoomWeakpoint showcaseRoomWeakpoint;
    }

    private void Awake()
    {
        InitializePlayerGameObject();
    }

    protected virtual void Start()
    {
        SetIsHit(false,false);
    }

    private void Update()
    {
        HandleWeakPointPower();
    }

    private void InitializePlayerGameObject()
    {
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
    }

    protected abstract void HandleWeakPointPower();

    public void SetWeakPoint(bool enable) => isEnabled = enable;
    public void SetIsHit(bool isHit, bool triggerEvents)
    {
        this.isHit = isHit;

        if (!triggerEvents) return;

        if (isHit)
        {
            OnWeakpointHit?.Invoke(this, EventArgs.Empty);
            OnAnyWeakpointHit?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnWeakpointUnHit?.Invoke(this, EventArgs.Empty);
            OnAnyWeakpointUnHit?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckEnableA(List<ShowcaseRoomWeakpoint> weakPointsToEnable) //Enable Visual
    {
        if (weakPointsToEnable.Contains(this))
        {
            OnWeakPointEnableA?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckEnableB(List<ShowcaseRoomWeakpoint> weakPointsToEnable) //Enable Functionality
    {
        if (weakPointsToEnable.Contains(this))
        {
            SetWeakPoint(true);
            OnShowcaseRoomWeakpointEnable?.Invoke(this, new OnShowcaseRoomWeakPointEventArgs { showcaseRoomWeakpoint = this });
            OnWeakPointEnableB?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckDisableA(List<ShowcaseRoomWeakpoint> weakPointsToDisable) //Disable Functionality
    {
        if (weakPointsToDisable.Contains(this))
        {
            SetWeakPoint(false);
            OnShowcaseRoomWeakpointDisable?.Invoke(this, new OnShowcaseRoomWeakPointEventArgs { showcaseRoomWeakpoint = this });
            OnWeakPointDisableA?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckDisableB(List<ShowcaseRoomWeakpoint> weakPointsToDisable) //Disable Visual
    {
        if (weakPointsToDisable.Contains(this))
        {
            OnWeakPointDisableB?.Invoke(this, EventArgs.Empty);
        }
    }

    protected bool CheckPlayerClose()
    {
        if (!player) return true;
        if (Vector3.Distance(transform.position, player.transform.position) <= PLAYER_DISTANCE_TO_HIT) return true;

        return false;
    }

    #region ShowcaseRoomWeakpointsHandler Subscriptions

    private void ShowcaseRoomsWeakPointsHandler_OnWeakPointsEnableA(object sender, ShowcaseRoomWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckEnableA(e.weakPoints);
    }
    private void ShowcaseRoomsWeakPointsHandler_OnWeakPointsEnableB(object sender, ShowcaseRoomWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckEnableB(e.weakPoints);
    }

    private void ShowcaseRoomsWeakPointsHandler_OnWeakPointsDisableA(object sender, ShowcaseRoomWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckDisableA(e.weakPoints);
    }

    private void ShowcaseRoomsWeakPointsHandler_OnWeakPointsDisableB(object sender, ShowcaseRoomWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckDisableB(e.weakPoints);
    }
    #endregion
}
