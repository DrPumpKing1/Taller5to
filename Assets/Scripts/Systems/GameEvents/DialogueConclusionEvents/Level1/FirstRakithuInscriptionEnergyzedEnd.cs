using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class FirstRakithuInscriptionEnergyzedEnd : DialogueConclusionEvent
{
    public static event EventHandler OnFirstRakithuInscriptionEnergyzedEnd;

    protected override void TriggerEvent()
    {
        OnFirstRakithuInscriptionEnergyzedEnd?.Invoke(this, EventArgs.Empty);
    }
}
