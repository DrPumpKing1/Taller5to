using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ShieldPieceUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShieldPieceSO shieldPieceSO;

    [Header("UI Components")]
    [SerializeField] private Image shieldPieceImage;

    private void OnEnable()
    {
        ShieldPiecesManager.OnShieldPieceCollected += ShieldPiecesManager_OnShieldPieceCollected;
    }
    private void OnDisable()
    {
        ShieldPiecesManager.OnShieldPieceCollected -= ShieldPiecesManager_OnShieldPieceCollected;
    }

    private void Start()
    {
        CheckShieldPieceImageState();
    }

    private void CheckShieldPieceImageState()
    {
        if (ShieldPiecesManager.Instance.ShieldPiecesCollected.Contains(shieldPieceSO))
        {
            ShowShieldPieceImage();
        }
        else
        {
            HideShieldPieceImage();
        }
    }

    private void ShowShieldPieceImage() => shieldPieceImage.gameObject.SetActive(true);
    private void HideShieldPieceImage() => shieldPieceImage.gameObject.SetActive(false);

    private void ShieldPiecesManager_OnShieldPieceCollected(object sender, ShieldPiecesManager.OnShieldPieceCollectedEventArgs e)
    {
        CheckShieldPieceImageState();
    }

}
