using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IncriptionTranslationUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InscriptionTranslation inscriptionTranslation;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI inscriptionTitle;
    [SerializeField] private TextMeshProUGUI inscriptionText;
    [SerializeField] private Image inscriptionImage;

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        SetInscriptionTitle();
        SetInscriptionText();
        SetInscriptionImage();
    }

    private void SetInscriptionTitle() => inscriptionTitle.text = inscriptionTranslation.Inscription.InscriptionSO.title;
    private void SetInscriptionText() => inscriptionText.text = inscriptionTranslation.Inscription.InscriptionSO.text;
    private void SetInscriptionImage() => inscriptionImage.sprite = inscriptionTranslation.Inscription.InscriptionSO.image;
}
