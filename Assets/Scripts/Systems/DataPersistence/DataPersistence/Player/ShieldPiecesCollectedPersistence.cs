using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPiecesCollectedPersistence : MonoBehaviour, IDataPersistence<PlayerData>
{
    public void LoadData(PlayerData data)
    {
        ShieldPiecesManager shieldPiecesManager = FindObjectOfType<ShieldPiecesManager>();

        foreach (KeyValuePair<int, bool> shieldPieceCollected in data.shieldPiecesCollected)
        {
            if (shieldPieceCollected.Value) shieldPiecesManager.AddShieldPieceToInventoryByID(shieldPieceCollected.Key);
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