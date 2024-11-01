using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShieldPieceCollectedVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShieldPieceCollection shieldPieceCollection;
    [SerializeField] private VisualEffect shieldPieceCollectionVFX;

    [Header("Settings")]
    [SerializeField] private bool detachAsCollected;
    [SerializeField, Range(1f, 5f)] private float detachedLifespan;

    private bool hasUnsubscribed;

    private void OnEnable()
    {
        shieldPieceCollection.OnShieldPieceCollected += ShieldPieceCollection_OnShieldPieceCollected;
    }

    private void OnDisable()
    {
        UnsubscribeFromShieldPieceCollectionEvents();
    }

    private void Start()
    {
        InitializeVariables();
        InitializeVFX();
    }

    private void InitializeVariables()
    {
        hasUnsubscribed = false;
    }

    private void InitializeVFX()
    {
        shieldPieceCollectionVFX.gameObject.SetActive(true);
        shieldPieceCollectionVFX.Stop();
    }

    private void StartVFX() => shieldPieceCollectionVFX.Play();

    private void StopVFX() => shieldPieceCollectionVFX.Stop();

    private void DetachVFX() => transform.parent = null;

    private void UnsubscribeFromShieldPieceCollectionEvents()
    {
        if (hasUnsubscribed) return;

        shieldPieceCollection.OnShieldPieceCollected -= ShieldPieceCollection_OnShieldPieceCollected;
        hasUnsubscribed = true;
    }

    private void DestroyAfterLifespan() => Destroy(gameObject, detachedLifespan);


    #region ShieldPieceCollection Subscriptions
    private void ShieldPieceCollection_OnShieldPieceCollected(object sender, ShieldPieceCollection.OnShieldPieceCollectedEventArgs e)
    {
        StartVFX();

        if (detachAsCollected)
        {
            DetachVFX();
            UnsubscribeFromShieldPieceCollectionEvents();
            DestroyAfterLifespan();
        }
    }
    #endregion
}
