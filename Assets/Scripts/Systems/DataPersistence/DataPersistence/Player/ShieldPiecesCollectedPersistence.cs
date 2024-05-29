using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPiecesCollectedPersistence : MonoBehaviour, IDataPersistence<PlayerData>
{
    public void LoadData(PlayerData data)
    {
        ShieldPiecesManager shieldPiecesManager = FindObjectOfType<ShieldPiecesManager>();
        Inscription[] inscriptions = FindObjectsOfType<Inscription>();

        foreach (KeyValuePair<int, bool> shieldPieceCollected in data.shieldPiecesCollected)
        {
            if (shieldPieceCollected.Value)
            {
                shieldPiecesManager.AddShieldPieceToInventoryByID(shieldPieceCollected.Key);
            }

            //For shield pieces in inscriptions

            Inscription shieldPieceRelatedInscription = null;

            foreach(Inscription inscription in inscriptions)
            {
                if(shieldPieceCollected.Key == inscription.ShieldPieceSO.id)
                {
                    shieldPieceRelatedInscription = inscription;
                    break;
                }
            }

            if (!shieldPieceRelatedInscription)
            {
                Debug.Log($"There is not an inscription with shield piece of ID: {shieldPieceCollected.Key}");
                continue;
            }

            if (shieldPieceCollected.Value)
            {
                shieldPieceRelatedInscription.SetHasDroppedShieldPiece(true);
            }
            else
            {
                shieldPieceRelatedInscription.SetHasDroppedShieldPiece(false);
            }

        }
    }

    public void SaveData(ref PlayerData data)
    {
        ShieldPiecesManager shieldPiecesManager = FindObjectOfType<ShieldPiecesManager>();

        foreach (ShieldPieceSO shieldPiece in shieldPiecesManager.CompleteShieldPiecesPool) //Clear all data in data
        {
            if (data.shieldPiecesCollected.ContainsKey(shieldPiece.id)) data.shieldPiecesCollected.Remove(shieldPiece.id);
        }

        foreach (ShieldPieceSO shieldPiece in shieldPiecesManager.CompleteShieldPiecesPool)
        {
            bool collected = shieldPiecesManager.CheckIfInventoryContainsShieldPiece(shieldPiece);

            data.shieldPiecesCollected.Add(shieldPiece.id, collected);
        }
    }
}