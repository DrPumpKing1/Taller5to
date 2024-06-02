using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPiece : MonoBehaviour
{
    [Header("Shield Piede Settings")]
    [SerializeField] private ShieldPieceSO shieldPieceSO;

    [Header("Identifiers")]
    [SerializeField] private bool isCollected;

    public ShieldPieceSO ShieldPieceSO => shieldPieceSO;
    public bool IsCollected => isCollected;

    private void Awake()
    {
        Physics.IgnoreLayerCollision(6, 17);
        Physics.IgnoreLayerCollision(8, 17);
    }

    public void SetIsCollected(bool collected) => isCollected = collected;
}
