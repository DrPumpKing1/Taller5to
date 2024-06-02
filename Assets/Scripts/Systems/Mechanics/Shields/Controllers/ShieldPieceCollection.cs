using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShieldPieceCollection : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] private ShieldPiece shieldPiece;
    [SerializeField] private Transform model;

    [Header("Interactable Settings")]
    [SerializeField, Range(1f, 100f)] private float horizontalInteractionRange;
    [SerializeField, Range(1f, 100f)] private float verticalInteractionRange;
    [Space]
    [SerializeField] private bool canBeSelected;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool hasAlreadyBeenInteracted;
    [Space]
    [SerializeField] private string tooltipMessage;
    [Space]
    [SerializeField] private bool grabPetAttention;
    [SerializeField] private bool grabPlayerAttention;

    [Header("Proximity Collection")]
    [SerializeField] private bool enableProximityCollection;
    [SerializeField] private float proximityRadius;

    private GameObject player;
    private const string PLAYER_TAG = "Player";

    #region IInteractable Properties
    public float HorizontalInteractionRange => horizontalInteractionRange;
    public float VerticalInteractionRange => verticalInteractionRange;
    public bool IsSelectable => canBeSelected;
    public bool IsInteractable => isInteractable;
    public bool HasAlreadyBeenInteracted => hasAlreadyBeenInteracted;
    public string TooltipMessage => tooltipMessage;
    public bool GrabPetAttention => grabPetAttention;
    public bool GrabPlayerAttention => grabPlayerAttention;
    #endregion

    #region IInteractableEvents
    public event EventHandler OnObjectSelected;
    public event EventHandler OnObjectDeselected;
    public event EventHandler OnObjectInteracted;
    public event EventHandler OnObjectFailInteracted;
    public event EventHandler OnObjectHasAlreadyBeenInteracted;
    public event EventHandler OnUpdatedInteractableState;
    #endregion

    public event EventHandler<OnShieldPieceCollectedEventArgs> OnShieldPieceCollected;

    public class OnShieldPieceCollectedEventArgs : EventArgs
    {
        public ShieldPieceSO shieldPieceSO;
    }

    private void Start()
    {
        InitializeVariables();
    }
    private void Update()
    {
        CheckProximityCollection();
    }

    private void InitializeVariables()
    {
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
    }

    private void CheckProximityCollection()
    {
        if (!enableProximityCollection) return;
        if (shieldPiece.IsCollected) return;
        if (!(Vector3.Distance(transform.position, player.transform.position) <= proximityRadius)) return;

        CollectShieldPiece();
    }

    #region  IInteractable Methods
    public void Interact()
    {
        //Debug.Log(gameObject.name + " Interacted");
        OnObjectInteracted?.Invoke(this, EventArgs.Empty);

        CollectShieldPiece();
    }

    public void FailInteract()
    {
        Debug.Log(gameObject.name + " Fail Interacted");
        OnObjectFailInteracted?.Invoke(this, EventArgs.Empty);
    }

    public void AlreadyInteracted()
    {
        Debug.Log(gameObject.name + " Has Already Been Interacted");
        OnObjectHasAlreadyBeenInteracted?.Invoke(this, EventArgs.Empty);
    }

    public void Select()
    {
        //Enable some UI feedback
        OnObjectSelected?.Invoke(this, EventArgs.Empty);
    }

    public void Deselect()
    {
        //Disable some UI feedback
        OnObjectDeselected?.Invoke(this, EventArgs.Empty);
    }

    public void TryInteract()
    {
        if (hasAlreadyBeenInteracted)
        {
            AlreadyInteracted();
            return;
        }

        if (!isInteractable)
        {
            FailInteract();
            return;
        }

        Interact();
    }

    public Transform GetTransform() => transform;
    #endregion

    private void CollectShieldPiece()
    {
        canBeSelected = false;
        isInteractable = false;
        hasAlreadyBeenInteracted = true;

        AddShieldPieceToInventory();
        DisableModel();

        shieldPiece.SetIsCollected(true);
        OnShieldPieceCollected?.Invoke(this, new OnShieldPieceCollectedEventArgs { shieldPieceSO = shieldPiece.ShieldPieceSO });
        OnUpdatedInteractableState?.Invoke(this, EventArgs.Empty);
    }

    private void DisableModel() => model.gameObject.SetActive(false);

    private void AddShieldPieceToInventory() => ShieldPiecesManager.Instance.CollectShieldPiece(shieldPiece.ShieldPieceSO);
}