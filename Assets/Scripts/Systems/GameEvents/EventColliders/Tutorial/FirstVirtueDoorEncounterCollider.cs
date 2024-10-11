using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstVirtueDoorEncounterCollider : ConditionalEventCollider
{
    protected override bool MeetsCondition() => !ShieldPiecesManager.Instance.HasCompletedShield(Dialect.Zurryth);
}

