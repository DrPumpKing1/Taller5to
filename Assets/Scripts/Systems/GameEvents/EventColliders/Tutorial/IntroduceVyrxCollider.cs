using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IntroduceVyrxCollider : EventCollider
{
    public static event EventHandler OnIntroduceVyrx;

    protected override void TriggerCollider()
    {
        OnIntroduceVyrx?.Invoke(this, EventArgs.Empty);
    }
}
