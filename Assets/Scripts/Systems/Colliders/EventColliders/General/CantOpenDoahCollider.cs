using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CantOpenDoahCollider : ConditionalEventCollider
{
    [Header("Settings")]
    [SerializeField] private Dialect dialect;

    [Header("Dev Settings")]
    [SerializeField] private bool devTeamEnabled;

    public class OnCantOpenDoahColliderEventArgs : EventArgs
    {
        public Dialect dialect;
    }

    protected override bool MeetsCondition()
    {
        if (!devTeamEnabled) return false;
        return (!ShieldPiecesManager.Instance.HasCompletedShield(dialect));
    }
}
