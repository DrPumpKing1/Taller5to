using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldPiecesManager : MonoBehaviour
{
    public static ShieldPiecesManager Instance { get; private set; }

    [Header("Shield Pieces Settings")]
    [SerializeField] private List<ShieldPieceSO> shieldPiecesCollected;
    [SerializeField] private List<ShieldPieceSO> completeShieldPiecesPool;

    [Header("Debug")]
    [SerializeField] private bool debug;
    public List<ShieldPieceSO> ShieldPiecesCollected { get { return shieldPiecesCollected; } }
    public List<ShieldPieceSO> CompleteShieldPiecesPool { get { return completeShieldPiecesPool; } }

    public static event EventHandler<OnShieldPieceCollectedEventArgs> OnShieldPieceCollected;

    public class OnShieldPieceCollectedEventArgs : EventArgs
    {
        public ShieldPieceSO shieldPieceSO;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(HasCompletedShield(Dialect.Rakithu));
        }
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one ShieldPiecesManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void CollectShieldPiece(ShieldPieceSO collectedPiece)
    {
        AddPieceToInventory(collectedPiece);
        OnShieldPieceCollected?.Invoke(this, new OnShieldPieceCollectedEventArgs { shieldPieceSO = collectedPiece });
    }

    private void AddPieceToInventory(ShieldPieceSO pieceToCollect)
    {
        if (shieldPiecesCollected.Contains(pieceToCollect))
        {
            if (debug) Debug.Log($"Inventory already contains shield piece with name: {pieceToCollect.pieceName}");
            return;
        }

        shieldPiecesCollected.Add(pieceToCollect);
    }

    public void AddShieldPieceToInventoryByID(int id)
    {
        ShieldPieceSO pieceToCollect = GetShieldPieceInCompletePoolByID(id);

        if (!pieceToCollect)
        {
            if (debug) Debug.LogWarning("Addition will be ignored due to shield piece not found");
            return;
        }

        if (CheckIfInventoryContainsShieldPiece(pieceToCollect))
        {
            if (debug) Debug.Log($"Symbols Dictionary already contains symbolToAdd with id: {pieceToCollect.id}");
            return;
        }

        shieldPiecesCollected.Add(pieceToCollect);
    }

    public bool CheckIfInventoryContainsShieldPiece(ShieldPieceSO shieldPiece) => shieldPiecesCollected.Contains(shieldPiece);

    public bool CheckIfInventoryContainsShieldPieceByID(int id)
    {
        foreach (ShieldPieceSO shieldPiece in shieldPiecesCollected)
        {
            if (shieldPiece.id == id) return true;
        }

        return false;
    }

    public ShieldPieceSO GetShieldPieceInCompletePoolByID(int id)
    {
        foreach (ShieldPieceSO shieldPiece in completeShieldPiecesPool)
        {
            if (shieldPiece.id == id) return shieldPiece;
        }

        if (debug) Debug.LogWarning($"Shield piece with id {id} not found in completePool");
        return null;
    }

    public ShieldPieceSO GetShieldPieceInInventoryByID(int id)
    {
        foreach (ShieldPieceSO shieldPiece in shieldPiecesCollected)
        {
            if (shieldPiece.id == id) return shieldPiece;
        }
        return null;
    }

    public bool HasCompletedShield(Dialect dialect)
    {
        foreach(ShieldPieceSO shieldPieceSO in completeShieldPiecesPool)
        {
            if (dialect != shieldPieceSO.dialect) continue;
            if (!shieldPiecesCollected.Contains(shieldPieceSO)) return false;
        }

        return true;
    }

    public int GetNumberOfPiecesByDialect(Dialect dialect)
    {
        int count = 0;

        foreach(ShieldPieceSO shieldPieceSO in shieldPiecesCollected)
        {
            if (shieldPieceSO.dialect == dialect) count++;
        }

        return count;
    }

    public void ReplaceShieldPiecesCollectedList(List<ShieldPieceSO> shieldPiecesSOs)
    {
        shieldPiecesCollected.Clear();

        foreach (ShieldPieceSO shieldPieceSO in shieldPiecesCollected)
        {
            if (!shieldPiecesCollected.Contains(shieldPieceSO)) continue;
            shieldPiecesCollected.Add(shieldPieceSO);
        }
    }
}
