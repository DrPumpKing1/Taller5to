using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInscriptionInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FirstRakithuInscriptionEnergyzedEnd.OnFirstRakithuInscriptionEnergyzedEnd += FirstRakithuInscriptionEnergyzedEnd_OnFirstRakithuInscriptionEnergyzedEnd; ;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FirstRakithuInscriptionEnergyzedEnd.OnFirstRakithuInscriptionEnergyzedEnd -= FirstRakithuInscriptionEnergyzedEnd_OnFirstRakithuInscriptionEnergyzedEnd; ;

    }

    private void FirstRakithuInscriptionEnergyzedEnd_OnFirstRakithuInscriptionEnergyzedEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}