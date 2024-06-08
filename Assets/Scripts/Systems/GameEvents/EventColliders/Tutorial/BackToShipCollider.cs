using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackToShipCollider : EventCollider
{
    public static event EventHandler OnBackToShip;

    protected override void TriggerCollider()
    {
        OnBackToShip?.Invoke(this, EventArgs.Empty);
    }
}
