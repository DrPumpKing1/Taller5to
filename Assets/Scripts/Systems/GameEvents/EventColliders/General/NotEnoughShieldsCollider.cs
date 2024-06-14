using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotEnoughShieldsCollider : ConditionalEventCollider
{
    [Header("Settings")]
    [SerializeField] private Dialect dialect;

    public static event EventHandler<OnNotEnoughShieldsColliderEventArgs> OnNotEnoughShieldsCollider;

    public class OnNotEnoughShieldsColliderEventArgs : EventArgs
    {
        public Dialect dialect;
    }

    protected override void TriggerCollider()
    {
        if (!MeetsCondition()) return;
        OnNotEnoughShieldsCollider?.Invoke(this, new OnNotEnoughShieldsColliderEventArgs { dialect = dialect });
    }

    protected override bool MeetsCondition()
    {
        if (!ShieldPiecesManager.Instance.HasCompletedShield(dialect)) return true;
        return false;
    }
}
