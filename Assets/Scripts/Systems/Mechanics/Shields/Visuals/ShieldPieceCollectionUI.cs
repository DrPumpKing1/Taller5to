using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShieldPieceCollectionUI : FeedbackUI
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI shieldPieceCollectedText;

    public void SetShieldPieceCollectionText(ShieldPieceSO shieldPiece)
    {
        shieldPieceCollectedText.text = $"{shieldPiece.dialect} shield piece collected";
    }
}
