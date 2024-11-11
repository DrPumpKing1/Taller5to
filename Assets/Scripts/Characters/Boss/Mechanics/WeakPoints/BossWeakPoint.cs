using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossWeakPoint : MonoBehaviour
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

    public static event EventHandler<OnBossWeakPointEventArgs> OnBossWeakpointEnable;
    public static event EventHandler<OnBossWeakPointEventArgs> OnBossWeakpointDisable;

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
        BossWeakPointsHandler.OnWeakPointsEnableA += BossWeakPointsHandler_OnWeakPointsEnableA;
        BossWeakPointsHandler.OnWeakPointsEnableB += BossWeakPointsHandler_OnWeakPointsEnableB;
        BossWeakPointsHandler.OnWeakPointsDisableA += BossWeakPointsHandler_OnWeakPointsDisableA;
        BossWeakPointsHandler.OnWeakPointsDisableB += BossWeakPointsHandler_OnWeakPointsDisableB;
    }

    private void OnDisable()
    {
        BossWeakPointsHandler.OnWeakPointsEnableA -= BossWeakPointsHandler_OnWeakPointsEnableA;
        BossWeakPointsHandler.OnWeakPointsEnableB -= BossWeakPointsHandler_OnWeakPointsEnableB;
        BossWeakPointsHandler.OnWeakPointsDisableA -= BossWeakPointsHandler_OnWeakPointsDisableA;
        BossWeakPointsHandler.OnWeakPointsDisableB -= BossWeakPointsHandler_OnWeakPointsDisableB;
    }

    public class OnBossWeakPointEventArgs : EventArgs
    {
        public BossWeakPoint bossWeakPoint;
    }

    private void Awake()
    {
        InitializePlayerGameObject();
    }

    protected virtual void Start()
    {
        SetIsHit(false, false);
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

    private void CheckEnableA(List<BossWeakPoint> weakPointsToEnable)
    {
        if (weakPointsToEnable.Contains(this))
        {
            OnWeakPointEnableA?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckEnableB(List<BossWeakPoint> weakPointsToEnable)
    {
        if(weakPointsToEnable.Contains(this))
        {
            SetWeakPoint(true);
            OnBossWeakpointEnable?.Invoke(this, new OnBossWeakPointEventArgs { bossWeakPoint = this });
            OnWeakPointEnableB?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckDisableA(List<BossWeakPoint> weakPointsToDisable)
    {
        if (weakPointsToDisable.Contains(this))
        {
            SetWeakPoint(false);
            OnBossWeakpointDisable?.Invoke(this, new OnBossWeakPointEventArgs { bossWeakPoint = this });
            OnWeakPointDisableA?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckDisableB(List<BossWeakPoint> weakPointsToDisable)
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

    #region BossWeakPointsHandler Subscriptions

    private void BossWeakPointsHandler_OnWeakPointsEnableA(object sender, BossWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckEnableA(e.weakPoints);
    }

    private void BossWeakPointsHandler_OnWeakPointsEnableB(object sender, BossWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckEnableB(e.weakPoints);
    }

    private void BossWeakPointsHandler_OnWeakPointsDisableA(object sender, BossWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckDisableA(e.weakPoints);
    }
    private void BossWeakPointsHandler_OnWeakPointsDisableB(object sender, BossWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckDisableB(e.weakPoints);
    }
    #endregion
}
