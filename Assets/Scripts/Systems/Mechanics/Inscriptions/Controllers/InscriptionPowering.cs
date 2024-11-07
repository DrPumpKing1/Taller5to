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
    [SerializeField] private Vector3 shieldRotation;
    [SerializeField] private bool shieldParenting;

    [Header("State")]
    [SerializeField] private bool state;

    private bool IsPowered => electrode.Power >= Electrode.ACTIVATION_THRESHOLD;
    private float notPoweredTimer;
    private bool previousPowered;
    private const float NOT_POWERED_TIME_THRESHOLD = 0.2f;

    public static event EventHandler<OnInscriptionPoweringEventArgs> OnInscriptionPoweringFirstTime;

    public static event EventHandler<OnInscriptionPoweringEventArgs> OnInscriptionPower;
    public static event EventHandler<OnInscriptionPoweringEventArgs> OnInscriptionDePower;

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
        ManageState();
        HandlePowered();
    }

    private void InitializeVariables()
    {
        previousPowered = IsPowered;
    }


    private void ManageState()
    {
        if (state == IsPowered) return;

        if (state) state = false;
        else
        {
            state = true;
        }
    }

    private void HandlePowered()
    {
        if (!IsPowered)
        {
            notPoweredTimer += Time.deltaTime;
            
            if (notPoweredTimer >= NOT_POWERED_TIME_THRESHOLD && previousPowered)
            {
                OnInscriptionDePower?.Invoke(this, new OnInscriptionPoweringEventArgs { id = inscription.InscriptionSO.id });
                previousPowered = false;
            }
        }
        else
        {
            if (!previousPowered)
            {
                OnInscriptionPower?.Invoke(this, new OnInscriptionPoweringEventArgs { id = inscription.InscriptionSO.id });
                CheckDropShield();
            }

            notPoweredTimer = 0;
            previousPowered = true;
        }
    }

    private void CheckDropShield()
    {
        if (!inscription.ShouldDropShieldPiece()) return;

        DropShieldPiece();
        OnInscriptionPoweringFirstTime?.Invoke(this, new OnInscriptionPoweringEventArgs { id = inscription.InscriptionSO.id });
    }

    private void DropShieldPiece()
    {
        Transform shieldPieceTransform = Instantiate(inscription.ShieldPieceSO.prefab, dropPoint.position, Quaternion.Euler(shieldRotation));
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
