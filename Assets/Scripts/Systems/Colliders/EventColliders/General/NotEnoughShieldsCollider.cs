using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NotEnoughShieldsCollider : ConditionalEventCollider
{
    [Header("Settings")]
    [SerializeField] private Dialect dialect;

    public class OnNotEnoughShieldsColliderEventArgs : EventArgs
    {
        public Dialect dialect;
    }

    protected override bool MeetsCondition()
    {
        return (!ShieldPiecesManager.Instance.HasCompletedShield(dialect));
    }
}



