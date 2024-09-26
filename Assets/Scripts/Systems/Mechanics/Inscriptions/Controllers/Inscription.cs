using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscription : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InscriptionSO inscriptionSO;
    [SerializeField] private ShieldPieceSO shieldPieceSO;

    [Header("Settings")]
    [SerializeField] private bool hasBeenRead;
    [SerializeField] private bool hasDroppedShieldPiece;

    public InscriptionSO InscriptionSO => inscriptionSO;
    public ShieldPieceSO ShieldPieceSO => shieldPieceSO;

    public bool HasBeenRead => hasBeenRead;
    public bool HasDroppedShieldPiece => hasDroppedShieldPiece;

    public void SetHasBeenRead(bool read) => hasBeenRead = read;
    public void SetHasDroppedShieldPiece(bool dropped) => hasDroppedShieldPiece = dropped;

    public bool ShouldDropShieldPiece()
    {
        if (hasDroppedShieldPiece) return false;
        if (ShieldPiecesManager.Instance.ShieldPiecesCollected.Contains(shieldPieceSO)) return false;

        return true;
    }
}
