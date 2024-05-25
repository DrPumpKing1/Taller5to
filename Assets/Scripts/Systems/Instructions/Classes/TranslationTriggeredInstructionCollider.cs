using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TranslationInstruction : Instruction
{
    [Header("Components")]
    [SerializeField] private InscriptionTranslation inscriptionTranslation;

    [Header("Settings")]
    [SerializeField] private bool triggerOnAnyInscription;

    private void OnEnable()
    {
        InscriptionTranslation.OnAnyInscriptionTranslated += InscriptionTranslation_OnAnyInscriptionTranslated;
    }

    private void OnDisable()
    {
        InscriptionTranslation.OnAnyInscriptionTranslated -= InscriptionTranslation_OnAnyInscriptionTranslated;
    }

    private void HandleInstructionTrigger()
    {
        if (hasBeenTriggered) return;

        TriggerInstruction();
        hasBeenTriggered = true;
    }

    #region InscriptionTranslation Subscriptions
    private void InscriptionTranslation_OnAnyInscriptionTranslated(object sender, InscriptionTranslation.OnAnyInsctiptionTranslatedEventArgs e)
    {
        Debug.Log("Test");

        if (!triggerOnAnyInscription && e.inscriptionTranslation != inscriptionTranslation) return;
        HandleInstructionTrigger();
    }
    #endregion
}
