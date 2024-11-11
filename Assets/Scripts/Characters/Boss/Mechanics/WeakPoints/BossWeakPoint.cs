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

    public event EventHandler OnWeakPointEnable;
    public event EventHandler OnWeakPointDisable;

    public static event EventHandler OnAnyWeakpointUnHit;
    public static event EventHandler OnAnyWeakpointHit;
    public event EventHandler OnWeakpointHit;
    public event EventHandler OnWeakpointUnHit;

    private void OnEnable()
    {
        BossWeakPointsHandler.OnWeakPointsEnableB += BossWeakPointsHandler_OnWeakPointsEnable;
        BossWeakPointsHandler.OnWeakPointsDisableA += BossWeakPointsHandler_OnWeakPointsDisable;
    }

    private void OnDisable()
    {
        BossWeakPointsHandler.OnWeakPointsEnableB -= BossWeakPointsHandler_OnWeakPointsEnable;
        BossWeakPointsHandler.OnWeakPointsDisableA -= BossWeakPointsHandler_OnWeakPointsDisable;
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

    private void CheckEnable(List<BossWeakPoint> weakPointsToEnable)
    {
        if(weakPointsToEnable.Contains(this))
        {
            SetWeakPoint(true);
            OnBossWeakpointEnable?.Invoke(this, new OnBossWeakPointEventArgs { bossWeakPoint = this });
            OnWeakPointEnable?.Invoke(this, EventArgs.Empty);
        }
    }

    private void CheckDisable(List<BossWeakPoint> weakPointsToDisable)
    {
        if (weakPointsToDisable.Contains(this))
        {
            SetWeakPoint(false);
            OnBossWeakpointDisable?.Invoke(this, new OnBossWeakPointEventArgs { bossWeakPoint = this });
            OnWeakPointDisable?.Invoke(this, EventArgs.Empty);
        }
    }

    protected bool CheckPlayerClose()
    {
        if (!player) return true;
        if (Vector3.Distance(transform.position, player.transform.position) <= PLAYER_DISTANCE_TO_HIT) return true;

        return false;
    }

    #region BossWeakPointsHandler Subscriptions
    private void BossWeakPointsHandler_OnWeakPointsEnable(object sender, BossWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckEnable(e.weakPoints);
    }

    private void BossWeakPointsHandler_OnWeakPointsDisable(object sender, BossWeakPointsHandler.OnWeakPointsEventArgs e)
    {
        CheckDisable(e.weakPoints);
    }
    #endregion
}
