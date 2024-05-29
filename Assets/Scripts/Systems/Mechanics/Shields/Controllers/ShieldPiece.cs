using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPiece : MonoBehaviour
{
    [Header("Shield Piede Settings")]
    [SerializeField] private ShieldPieceSO shieldPieceSO;

    public ShieldPieceSO ShieldPieceSO => shieldPieceSO;

    private void Awake()
    {
        Physics.IgnoreLayerCollision(6, 17);
        Physics.IgnoreLayerCollision(8, 17);
    }
}
