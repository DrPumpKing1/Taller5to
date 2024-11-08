using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ShowcaseRoomWeakpoint : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;

    [Header("Components")]
    [SerializeField] private GameObject visual;

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

    public event EventHandler OnWeakPointEnable;
    public event EventHandler OnWeakPointDisable;

    private void OnEnable()
    {
        //ShowcaseRoomWeakPointsHandler.OnWeakPointsEnable += ShowcaseRoomWeakPointsHandler_OnWeakPointsEnable;
        //ShowcaseRoomWeakPointsHandler.OnWeakPointsDisable += ShowcaseRoomWeakPointsHandler_OnWeakPointsDisable;
    }

    private void OnDisable()
    {
        //ShowcaseRoomWeakPointsHandler.OnWeakPointsEnable -= ShowcaseRoomWeakPointsHandler_OnWeakPointsEnable;
        //ShowcaseRoomWeakPointsHandler.OnWeakPointsDisable -= ShowcaseRoomWeakPointsHandler_OnWeakPointsDisable;
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
        SetIsHit(false);
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

    public void SetWeakPoint(bool enable)
    {
        isEnabled = enable;
        SetVisual(enable);
    }
    public void SetIsHit(bool isHit) => this.isHit = isHit;

    private void CheckEnable(List<ShowcaseRoomWeakpoint> weakPointsToEnable)
    {
        if (weakPointsToEnable.Contains(this))
        {
            SetWeakPoint(true);
            OnShowcaseRoomWeakpointEnable?.Invoke(this, new OnShowcaseRoomWeakPointEventArgs { showcaseRoomWeakpoint = this });
            OnWeakPointEnable?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckDisable(List<ShowcaseRoomWeakpoint> weakPointsToDisable)
    {
        if (weakPointsToDisable.Contains(this))
        {
            SetWeakPoint(false);
            OnShowcaseRoomWeakpointDisable?.Invoke(this, new OnShowcaseRoomWeakPointEventArgs { showcaseRoomWeakpoint = this });
            OnWeakPointDisable?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetVisual(bool active) => visual.SetActive(active);

    protected bool CheckPlayerClose()
    {
        if (!player) return true;
        if (Vector3.Distance(transform.position, player.transform.position) <= PLAYER_DISTANCE_TO_HIT) return true;

        return false;
    }

    #region ShowcaseRoomWeakpointsHandler Subscriptions
    private void BossWeakPointsHandler_OnWeakPointsEnable(object sender, BossWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        //CheckEnable(e.weakPoints);
    }

    private void BossWeakPointsHandler_OnWeakPointsDisable(object sender, BossWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        //CheckDisable(e.weakPoints);
    }
    #endregion
}
