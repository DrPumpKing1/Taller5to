using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscription : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InscriptionSO inscriptionSO;

    public InscriptionSO InscriptionSO { get { return inscriptionSO; } }
}
