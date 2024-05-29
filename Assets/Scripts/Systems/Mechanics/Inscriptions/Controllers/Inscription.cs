using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscription : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InscriptionSO inscriptionSO;

    [Header("Identifiers")]
    [SerializeField] private bool hasBeenRead;
    [SerializeField] private bool hasDroppedShieldPiece;

    public InscriptionSO InscriptionSO => inscriptionSO;
    public bool HasBeenRead => hasBeenRead;
    public bool HasDroppedShieldPiece => hasDroppedShieldPiece;

    public void SetHasBeenRead(bool read) => hasBeenRead = read;
    public void SetHasDroppedShieldPiece(bool dropped) => hasDroppedShieldPiece = dropped;
}
