using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationInstruction : Instruction
{
    [Header("Components")]
    [SerializeField] private InscriptionRead inscriptionTranslation;

    [Header("Settings")]
    [SerializeField] private bool triggerOnAnyInscription;

    private void OnEnable()
    {
        InscriptionRead.OnAnyInscriptionTranslated += InscriptionTranslation_OnAnyInscriptionTranslated;
    }

    private void OnDisable()
    {
        InscriptionRead.OnAnyInscriptionTranslated -= InscriptionTranslation_OnAnyInscriptionTranslated;
    }

    #region InscriptionTranslation Subscriptions
    private void InscriptionTranslation_OnAnyInscriptionTranslated(object sender, InscriptionRead.OnAnyInsctiptionTranslatedEventArgs e)
    {
        if (!triggerOnAnyInscription && e.inscriptionRead != inscriptionTranslation) return;
        HandleInstructionTrigger();
    }
    #endregion
}
