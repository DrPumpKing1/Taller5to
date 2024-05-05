using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscription : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InscriptionSO inscriptionSO;

    [Header("Identifiers")]
    [SerializeField] private int id;
    [SerializeField] private bool isTranslated;

    public InscriptionSO InscriptionSO => inscriptionSO;
    public int ID => id;
    public bool IsTranslated => isTranslated;

    public void SetIsTranslated() => isTranslated = true;
}
