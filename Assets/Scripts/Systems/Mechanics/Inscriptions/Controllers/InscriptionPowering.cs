using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InscriptionPowering : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Inscription inscription;
    [SerializeField] private Electrode electrode;
    [SerializeField] private Transform dropPoint;

    [Header("Settings")]
    [SerializeField] private Vector3 shieldDropForce;
    [SerializeField] private bool shieldParenting;

    private bool Power => electrode.Power >= Electrode.ACTIVATION_THRESHOLD;

    private bool previousPowered;

    public static event EventHandler<OnInscriptionPoweringEventArgs> OnInscriptionPoweringFirstTime;

    public class OnInscriptionPoweringEventArgs : EventArgs
    {
        public int id;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckHasBeenPowered();
    }

    private void InitializeVariables()
    {
        previousPowered = Power;
    }

    private void CheckHasBeenPowered()
    {
        if(Power && !previousPowered)
        {
            CheckDropShield();
        }

        previousPowered = Power;
    }

    private void CheckDropShield()
    {
        if (inscription.HasDroppedShieldPiece) return;

        DropShieldPiece();
        OnInscriptionPoweringFirstTime?.Invoke(this, new OnInscriptionPoweringEventArgs { id = inscription.InscriptionSO.id });
    }

    private void DropShieldPiece()
    {
        Transform shieldPieceTransform = Instantiate(inscription.ShieldPieceSO.prefab, dropPoint.position, dropPoint.rotation);
        AddShieldPieceDropForce(shieldPieceTransform);

        if (shieldParenting) shieldPieceTransform.SetParent(dropPoint);

        inscription.SetHasDroppedShieldPiece(true);
    }

    private void AddShieldPieceDropForce(Transform shieldPieceTransform)
    {
        Rigidbody shieldPieceRigidbody = shieldPieceTransform.GetComponent<Rigidbody>();

        if (!shieldPieceRigidbody)
        {
            Debug.Log("ShieldPiece does not have a Rigidbody component");
            return;
        }

        shieldPieceRigidbody.AddForce(shieldDropForce, ForceMode.Impulse);
    }
}
