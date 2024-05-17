using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RoomVisibilityObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<RoomVisibilityCollider> controllingColliders;

    private List<Renderer> renderers = new List<Renderer>();

    private void OnEnable()
    {
        RoomVisibilityManager.OnStartBlockingViewColliders += RoomVisibilityManager_OnStartBlockingViewColliders;
        RoomVisibilityManager.OnEnterBlockingViewColliders += RoomVisibilityManager_OnEnterBlockingViewColliders;
        RoomVisibilityManager.OnExitBlockingViewColliders += RoomVisibilityManager_OnExitBlockingViewColliders;
    }

    private void OnDisable()
    {
        RoomVisibilityManager.OnStartBlockingViewColliders -= RoomVisibilityManager_OnStartBlockingViewColliders;
        RoomVisibilityManager.OnEnterBlockingViewColliders -= RoomVisibilityManager_OnEnterBlockingViewColliders;
        RoomVisibilityManager.OnExitBlockingViewColliders -= RoomVisibilityManager_OnExitBlockingViewColliders;
    }

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>().ToList();
    }

    private void EnableMeshRenderers()
    {
        foreach(Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }
    }

    private void DisableMeshRenderers()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

    private void CheckStartVisibility(List<RoomVisibilityCollider> currentCollidersBlockingView)
    {
        if (controllingColliders.Intersect(currentCollidersBlockingView).Any())
        {
            EnableMeshRenderers();
        }
        else
        {
            DisableMeshRenderers();
        }
    }

    private void CheckEnterVisibility(List<RoomVisibilityCollider> previousCollidersBlockingView, List<RoomVisibilityCollider> enterVisibilityColliders)
    {
        if ((controllingColliders.Intersect(previousCollidersBlockingView).Any())) return; //A controllingCollider was already on the previous List

        if (!(controllingColliders.Intersect(enterVisibilityColliders).Any())) return;

        EnableMeshRenderers();
    }

    private void CheckExitVisibility(List<RoomVisibilityCollider> previousCollidersBlockingView, List<RoomVisibilityCollider> currentVisibilityColliders)
    {
        if (!(controllingColliders.Intersect(previousCollidersBlockingView).Any())) return; //There wasn't any controllingCollider on previous list (none o them was bocking view and this was already invisible)
        //At this point, there was at least one collider blocking view

        if (controllingColliders.Intersect(currentVisibilityColliders).Any()) return; //If right now there is at least one collider blocking view, return
        //At this point, there was at least one collider blocking view, but now there is none

        DisableMeshRenderers();
    }

    #region RoomVisibilityManager Subscriptions
    private void RoomVisibilityManager_OnStartBlockingViewColliders(object sender, RoomVisibilityManager.OnBlockingViewCollidersStartEventArgs e)
    {
        CheckStartVisibility(e.currentRoomVisibilityColliders);
    }
    private void RoomVisibilityManager_OnEnterBlockingViewColliders(object sender, RoomVisibilityManager.OnBlockingViewCollidersEnterEventArgs e)
    {
        CheckEnterVisibility(e.previousRoomVisibilityColliders,e.newRoomVisibilityColliders);
    }

    private void RoomVisibilityManager_OnExitBlockingViewColliders(object sender, RoomVisibilityManager.OnBlockingViewCollidersExitEventArgs e)
    {
        CheckExitVisibility(e.previousRoomVisibilityColliders, e.currentRoomVisibilityColliders);
    }
    #endregion
}
