using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPieceCollectedVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShieldPieceCollection shieldPieceCollection;

    [Header("Learning Success Settings")]
    [SerializeField] private Transform shieldPieceCollectionUIPrefab;
    [SerializeField] private Vector3 instantiationPositionOffset;

    private void OnEnable()
    {
        shieldPieceCollection.OnShieldPieceCollected += ShieldPieceCollection_OnShieldPieceCollected;
    }
    private void OnDisable()
    {
        shieldPieceCollection.OnShieldPieceCollected -= ShieldPieceCollection_OnShieldPieceCollected;
    }

    private void ShieldPieceCollection_OnShieldPieceCollected(object sender, ShieldPieceCollection.OnShieldPieceCollectedEventArgs e)
    {
        Transform shieldPieceCollectionUITransform = Instantiate(shieldPieceCollectionUIPrefab, transform.position + instantiationPositionOffset, transform.rotation);

        ShieldPieceCollectionUI shieldPieceCollectionUI = shieldPieceCollectionUITransform.GetComponentInChildren<ShieldPieceCollectionUI>();

        if (!shieldPieceCollectionUI)
        {
            Debug.LogWarning("There's not a ShieldPieceCollectionUI attached to instantiated prefab");
            return;
        }

        shieldPieceCollectionUI.SetShieldPieceCollectionText(e.shieldPieceSO);
    }
}
