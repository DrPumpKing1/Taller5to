using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CantOpenDoahCollider : ConditionalEventCollider
{
    [Header("Settings")]
    [SerializeField] private Dialect dialect;

    public class OnCantOpenDoahColliderEventArgs : EventArgs
    {
        public Dialect dialect;
    }

    protected override bool MeetsCondition()
    {
        return (!ShieldPiecesManager.Instance.HasCompletedShield(dialect));
    }
}
