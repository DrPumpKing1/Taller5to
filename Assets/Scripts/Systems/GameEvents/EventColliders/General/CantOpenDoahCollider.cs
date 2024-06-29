using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CantOpenDoahCollider : ConditionalEventCollider
{
    [Header("Settings")]
    [SerializeField] private Dialect dialect;

    public static event EventHandler<OnCantOpenDoahColliderEventArgs> OnCantOpenDoahCollider;

    public class OnCantOpenDoahColliderEventArgs : EventArgs
    {
        public Dialect dialect;
    }

    protected override void TriggerCollider()
    {
        if (!MeetsCondition()) return;
        OnCantOpenDoahCollider?.Invoke(this, new OnCantOpenDoahColliderEventArgs { dialect = dialect });
    }

    protected override bool MeetsCondition()
    {
        if (!ShieldPiecesManager.Instance.HasCompletedShield(dialect)) return true;
        return false;
    }
}
