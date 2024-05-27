using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslationSuccessUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI translationSuccessText;

    [Header("Settings")]
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        DestroyAfterLifetime();
    }

    public void SetLearningSuccessText(InscriptionSO inscription)
    {
        translationSuccessText.text = $"{inscription.dialect} inscription translated";
    }

    private void DestroyAfterLifetime() => Destroy(transform.parent.gameObject, lifeTime);
}
