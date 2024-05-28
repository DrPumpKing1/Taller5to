using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationSuccessVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InscriptionRead inscriptionTranslation;

    [Header("Learning Success Settings")]
    [SerializeField] private Transform translationSuccessUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private void OnEnable()
    {
        inscriptionTranslation.OnInscriptionTranslated += InscriptionTranslation_OnInscriptionTranslated;    
    }
    private void OnDisable()
    {
        inscriptionTranslation.OnInscriptionTranslated -= InscriptionTranslation_OnInscriptionTranslated;
    }

    private void InscriptionTranslation_OnInscriptionTranslated(object sender, InscriptionRead.OnInscriptionTranslatedEventArgs e)
    {
        Transform translationSuccessUITransform = Instantiate(translationSuccessUIPrefab, transform.position + instantiationPositionOffset, transform.rotation);

        TranslationSuccessUI translationSuccessUI = translationSuccessUITransform.GetComponentInChildren<TranslationSuccessUI>();

        if (!translationSuccessUI)
        {
            Debug.LogWarning("There's not a TranslationSuccessUI attached to instantiated prefab");
            return;
        }

        translationSuccessUI.SetLearningSuccessText(e.inscriptionSO);
    }
}
