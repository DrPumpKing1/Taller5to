using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShieldPieceCollectionUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI shieldPieceCollectedText;

    [Header("Settings")]
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        DestroyAfterLifetime();
    }

    public void SetShieldPieceCollectionText(ShieldPieceSO shieldPiece)
    {
        shieldPieceCollectedText.text = $"{shieldPiece.dialect} shield piece collected";
    }

    private void DestroyAfterLifetime() => Destroy(transform.parent.gameObject, lifeTime);
}
