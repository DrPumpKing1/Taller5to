using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AncientRelicCollectedVFXHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AncientRelic ancientRelic;
    [SerializeField] private VisualEffect ancientRelicCollectedVFX;

    private void OnEnable()
    {
        ancientRelic.OnCollection += ShieldPieceCollection_OnShieldPieceCollected;
    }

    private void OnDisable()
    {
        ancientRelic.OnCollection -= ShieldPieceCollection_OnShieldPieceCollected;
    }

    private void Start()
    {
        InitializeVFX();
    }

    private void InitializeVFX()
    {
        ancientRelicCollectedVFX.gameObject.SetActive(true);
        StopVFX();
    }

    private void StartVFX() => ancientRelicCollectedVFX.Play();

    private void StopVFX() => ancientRelicCollectedVFX.Stop();


    #region ShieldPieceCollection Subscriptions
    private void ShieldPieceCollection_OnShieldPieceCollected(object sender, EventArgs e)
    {
        StartVFX();
    }
    #endregion
}
