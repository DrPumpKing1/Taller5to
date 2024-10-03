using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossWeakPoint : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;

    [Header("Settings")]
    [SerializeField] private bool weakPointEnabled;
    [SerializeField] private bool weakPointHit;

    public int ID => id;
    public bool WeakPointEnabled => weakPointEnabled;
    public bool WeakPointHit => weakPointHit;

    public static event EventHandler<OnBossWeakPointHitEventArgs> OnBossWeakPointHit;

    public class OnBossWeakPointHitEventArgs : EventArgs
    {
        public BossWeakPoint bossWeakPoint;
    }

    private void Update()
    {
        HandleWeakPointPower();
    }

    protected abstract void HandleWeakPointPower();

    protected void HitWeakPoint()
    {
        SetIsHit(true);
        SetWeakPoint(false);
        OnBossWeakPointHit?.Invoke(this, new OnBossWeakPointHitEventArgs { bossWeakPoint = this });
    }

    public void SetWeakPoint(bool enable) => weakPointEnabled = enable;
    public void SetIsHit(bool isHit) => weakPointHit = isHit;
}
