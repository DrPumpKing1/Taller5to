using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    public RoomManager Instance { get; private set; }

    [Header("Target Settings")]
    [SerializeField] private List<Transform> targets;

    [Header("Fade Object Settings")]
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private float raycastUpDistance;
    [SerializeField] private float raycastRayLenght;

    [Header("Read Only Data")]
    [SerializeField] private List<RoomCollider> roomCollidersBlockingView = new List<RoomCollider>();

    [Header("Debug")]
    [SerializeField] private bool debug;

    private List<RoomCollider> previousCollidersBlockingView = new List<RoomCollider>();

    public static event EventHandler<OnBlockingViewCollidersStartEventArgs> OnStartBlockingViewColliders;
    public static event EventHandler<OnBlockingViewCollidersEnterEventArgs> OnEnterBlockingViewColliders;
    public static event EventHandler<OnBlockingViewCollidersExitEventArgs> OnExitBlockingViewColliders;

    public static event EventHandler<OnRoomEventArgs> OnRoomEnter;
    public static event EventHandler<OnRoomEventArgs> OnRoomExit;

    public class OnRoomEventArgs : EventArgs
    {
        public int id;
        public string roomName;
    }

    public class OnBlockingViewCollidersStartEventArgs : EventArgs
    {
        public List<RoomCollider> currentRoomVisibilityColliders;
    }
    public class OnBlockingViewCollidersEnterEventArgs : EventArgs
    {
        public List<RoomCollider> previousRoomVisibilityColliders;
        public List<RoomCollider> newRoomVisibilityColliders;
    }

    public class OnBlockingViewCollidersExitEventArgs : EventArgs
    {
        public List<RoomCollider> previousRoomVisibilityColliders;
        public List<RoomCollider> currentRoomVisibilityColliders;
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
        roomCollidersBlockingView = GetCollidersBlockingView();
        previousCollidersBlockingView = roomCollidersBlockingView;

        OnStartBlockingViewColliders?.Invoke(this, new OnBlockingViewCollidersStartEventArgs { currentRoomVisibilityColliders = roomCollidersBlockingView });
    }

    private void HandleCollidersBlockingView()
    {
        roomCollidersBlockingView = GetCollidersBlockingView();

        List<RoomCollider> newCollidersBlockingView = roomCollidersBlockingView.Except(previousCollidersBlockingView).ToList();
        List<RoomCollider> oldCollidersBlockingView = previousCollidersBlockingView.Except(roomCollidersBlockingView).ToList();

        if(newCollidersBlockingView.Count > 0)
        {
            OnEnterBlockingViewColliders?.Invoke(this, new OnBlockingViewCollidersEnterEventArgs { previousRoomVisibilityColliders = previousCollidersBlockingView, newRoomVisibilityColliders = newCollidersBlockingView });
            OnRoomEnter?.Invoke(this, new OnRoomEventArgs { id = newCollidersBlockingView[0].ID, roomName = newCollidersBlockingView[0].RoomName });

        }

        if (oldCollidersBlockingView.Count > 0)
        {
            OnExitBlockingViewColliders?.Invoke(this, new OnBlockingViewCollidersExitEventArgs { previousRoomVisibilityColliders = previousCollidersBlockingView, currentRoomVisibilityColliders = roomCollidersBlockingView});
            OnRoomExit?.Invoke(this, new OnRoomEventArgs { id = oldCollidersBlockingView[0].ID, roomName = oldCollidersBlockingView[0].RoomName });
        }

        previousCollidersBlockingView = roomCollidersBlockingView;
    }

    private List<RoomCollider> GetCollidersBlockingView()
    {
        List<RoomCollider> collidersBlockingView = new List<RoomCollider>();

        foreach (Transform target in targets)
        {
            RaycastHit[] targetHits = new RaycastHit[10];

            Vector3 rayStartingPos = target.position + raycastUpDistance*Vector3.up;

            Physics.RaycastNonAlloc(rayStartingPos, Vector3.down, targetHits, raycastRayLenght, blockingLayer);

            if (debug) Debug.DrawRay(rayStartingPos, Vector3.down * (raycastRayLenght), Color.blue);

            foreach (RaycastHit hit in targetHits)
            {
                if (hit.collider == null) continue;

                if(hit.collider.TryGetComponent(out RoomCollider roomVisibilityCollider))
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
