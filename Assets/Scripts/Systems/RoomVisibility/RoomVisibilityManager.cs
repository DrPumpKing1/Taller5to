using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RoomVisibilityManager : MonoBehaviour
{
    public RoomVisibilityManager Instance { get; private set; }

    [Header("Target Settings")]
    [SerializeField] private List<Transform> targets;

    [Header("Fade Object Settings")]
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private float raycastUpDistance;
    [SerializeField] private float raycastRayLenght;

    [Header("Read Only Data")]
    [SerializeField] private List<RoomVisibilityCollider> collidersBlockingView = new List<RoomVisibilityCollider>();

    [Header("Debug")]
    [SerializeField] private bool debug;

    private List<RoomVisibilityCollider> previousCollidersBlockingView = new List<RoomVisibilityCollider>();

    public static event EventHandler<OnBlockingViewCollidersStartEventArgs> OnStartBlockingViewColliders;
    public static event EventHandler<OnBlockingViewCollidersEnterEventArgs> OnEnterBlockingViewColliders;
    public static event EventHandler<OnBlockingViewCollidersExitEventArgs> OnExitBlockingViewColliders;

    public class OnBlockingViewCollidersStartEventArgs : EventArgs
    {
        public List<RoomVisibilityCollider> currentRoomVisibilityColliders;
    }
    public class OnBlockingViewCollidersEnterEventArgs : EventArgs
    {
        public List<RoomVisibilityCollider> previousRoomVisibilityColliders;
        public List<RoomVisibilityCollider> newRoomVisibilityColliders;
    }

    public class OnBlockingViewCollidersExitEventArgs : EventArgs
    {
        public List<RoomVisibilityCollider> previousRoomVisibilityColliders;
        public List<RoomVisibilityCollider> currentRoomVisibilityColliders;
    }

    private void OnEnable()
    {
        PlayerStartPositioning.OnPlayerStartPositioned += PlayerStartPositioning_OnPlayerStartPositioned;       
    }

    private void OnDisable()
    {
        PlayerStartPositioning.OnPlayerStartPositioned -= PlayerStartPositioning_OnPlayerStartPositioned;
    }
    private void Awake()
    {
        SetSingleton();
    }
    private void Update()
    {
        HandleCollidersBlockingView();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one RoomVisibilityManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void CheckStartingCollidersBlockingView()
    {
        collidersBlockingView = GetCollidersBlockingView();
        previousCollidersBlockingView = collidersBlockingView;

        OnStartBlockingViewColliders?.Invoke(this, new OnBlockingViewCollidersStartEventArgs { currentRoomVisibilityColliders = collidersBlockingView });
    }

    private void HandleCollidersBlockingView()
    {
        collidersBlockingView = GetCollidersBlockingView();

        List<RoomVisibilityCollider> newCollidersBlockingView = collidersBlockingView.Except(previousCollidersBlockingView).ToList();
        List<RoomVisibilityCollider> oldCollidersBlockingView = previousCollidersBlockingView.Except(collidersBlockingView).ToList();

        if(newCollidersBlockingView.Count > 0)
        {
            OnEnterBlockingViewColliders?.Invoke(this, new OnBlockingViewCollidersEnterEventArgs { previousRoomVisibilityColliders = previousCollidersBlockingView, newRoomVisibilityColliders = newCollidersBlockingView }); ;
        }

        if (oldCollidersBlockingView.Count > 0)
        {
            OnExitBlockingViewColliders?.Invoke(this, new OnBlockingViewCollidersExitEventArgs { previousRoomVisibilityColliders = previousCollidersBlockingView, currentRoomVisibilityColliders = collidersBlockingView});
        }

        previousCollidersBlockingView = collidersBlockingView;
    }

    private List<RoomVisibilityCollider> GetCollidersBlockingView()
    {
        List<RoomVisibilityCollider> collidersBlockingView = new List<RoomVisibilityCollider>();

        foreach (Transform target in targets)
        {
            RaycastHit[] targetHits = new RaycastHit[10];

            Vector3 rayStartingPos = target.position + raycastUpDistance*Vector3.up;

            Physics.RaycastNonAlloc(rayStartingPos, Vector3.down, targetHits, raycastRayLenght, blockingLayer);

            if (debug) Debug.DrawRay(rayStartingPos, Vector3.down * (raycastRayLenght), Color.blue);

            foreach (RaycastHit hit in targetHits)
            {
                if (hit.collider == null) continue;

                if(hit.collider.TryGetComponent(out RoomVisibilityCollider roomVisibilityCollider))
                {
                    if (collidersBlockingView.Contains(roomVisibilityCollider)) continue;

                    collidersBlockingView.Add(roomVisibilityCollider);
                }
            }
        }

        return collidersBlockingView;
    }

    #region PlayerStartPositioning Subscriptions
    private void PlayerStartPositioning_OnPlayerStartPositioned(object sender, System.EventArgs e)
    {
        CheckStartingCollidersBlockingView();
    }
    #endregion
}
