using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region InscriptionTranslation Subscriptions
    private void InscriptionTranslation_OnAnyInscriptionTranslated(object sender, InscriptionTranslation.OnAnyInsctiptionTranslatedEventArgs e)
    {
        if (!triggerOnAnyInscription && e.inscriptionTranslation != inscriptionTranslation) return;
        HandleInstructionTrigger();
    }
    #endregion
}
