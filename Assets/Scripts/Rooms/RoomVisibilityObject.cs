using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class RoomVisibilityObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<RoomCollider> controllingColliders;

    private List<Renderer> renderers = new List<Renderer>();
    private List<Light> lights = new List<Light>();
    private List<Image> images = new List<Image>();

    private void OnEnable()
    {
        RoomManager.OnStartBlockingViewColliders += RoomVisibilityManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders += RoomVisibilityManager_OnEnterBlockingViewColliders;
        RoomManager.OnExitBlockingViewColliders += RoomVisibilityManager_OnExitBlockingViewColliders;
    }

    private void OnDisable()
    {
        RoomManager.OnStartBlockingViewColliders -= RoomVisibilityManager_OnStartBlockingViewColliders;
        RoomManager.OnEnterBlockingViewColliders -= RoomVisibilityManager_OnEnterBlockingViewColliders;
        RoomManager.OnExitBlockingViewColliders -= RoomVisibilityManager_OnExitBlockingViewColliders;
    }

    private void Awake()
    {
        UpdateRenderersList();
        UpdateLightsList();
        UpdateImagesList();
    }

    #region Renderers
    private void UpdateRenderersList()
    {
        renderers = GetComponentsInChildren<Renderer>(true).ToList();
    }

    private void EnableMeshRenderers()
    {
        UpdateRenderersList();

        foreach(Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }
    }

    private void DisableMeshRenderers()
    {
        UpdateRenderersList();

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }
    #endregion

    #region Lights
    private void UpdateLightsList()
    {
        lights = GetComponentsInChildren<Light>(true).ToList();
    }

    private void EnableLights()
    {
        UpdateLightsList();

        foreach (Light light in lights)
        {
            light.enabled = true;
        }
    }

    private void DisableLights()
    {
        UpdateLightsList();

        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }
    #endregion

    #region Images
    private void UpdateImagesList()
    {
        images = GetComponentsInChildren<Image>(true).ToList();
    }

    private void EnableImages()
    {
        UpdateImagesList();

        foreach (Image image in images)
        {
            image.enabled = true;
        }
    }

    private void DisableImages()
    {
        UpdateImagesList();

        foreach (Image image in images)
        {
            image.enabled = false;
        }
    }
    #endregion

    private void CheckStartVisibility(List<RoomCollider> currentCollidersBlockingView)
    {
        if (controllingColliders.Intersect(currentCollidersBlockingView).Any())
        {
            EnableMeshRenderers();
            EnableLights();
            EnableImages();
        }
        else
        {
            DisableMeshRenderers();
            DisableLights();
            DisableImages();
        }
    }

    private void CheckEnterVisibility(List<RoomCollider> previousCollidersBlockingView, List<RoomCollider> enterVisibilityColliders)
    {
        if ((controllingColliders.Intersect(previousCollidersBlockingView).Any())) return; //A controllingCollider was already on the previous List

        if (!(controllingColliders.Intersect(enterVisibilityColliders).Any())) return;

        EnableMeshRenderers();
        EnableLights();
        EnableImages();
    }

    private void CheckExitVisibility(List<RoomCollider> previousCollidersBlockingView, List<RoomCollider> currentVisibilityColliders)
    {
        if (!(controllingColliders.Intersect(previousCollidersBlockingView).Any())) return; //There wasn't any controllingCollider on previous list (none o them was bocking view and this was already invisible)
        //At this point, there was at least one collider blocking view

        if (controllingColliders.Intersect(currentVisibilityColliders).Any()) return; //If right now there is at least one collider blocking view, return
        //At this point, there was at least one collider blocking view, but now there is none

        DisableMeshRenderers();
        DisableLights();
        DisableImages();
    }

    #region RoomVisibilityManager Subscriptions
    private void RoomVisibilityManager_OnStartBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersStartEventArgs e)
    {
        CheckStartVisibility(e.currentRoomVisibilityColliders);
    }

    private void RoomVisibilityManager_OnEnterBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersEnterEventArgs e)
    {
        CheckEnterVisibility(e.previousRoomVisibilityColliders,e.newRoomVisibilityColliders);
    }

    private void RoomVisibilityManager_OnExitBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersExitEventArgs e)
    {
        CheckExitVisibility(e.previousRoomVisibilityColliders, e.currentRoomVisibilityColliders);
    }
    #endregion
}
