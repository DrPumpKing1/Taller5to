using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DirectionalLightingHandler : MonoBehaviour
{
    public static DirectionalLightingHandler Instance;

    [Header("Components")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private List<RoomCollider> controllingColliders;

    public bool overrideAlwaysOn;
    private bool shouldBeEnabled;

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
        SetSingleton();
    }

    private void Update()
    {
        HandleDirectionalLight();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CameraFollowPointHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandleDirectionalLight()
    {
        if (overrideAlwaysOn) 
        { 
            if(!directionalLight.enabled)
            {
                directionalLight.enabled = true;              
            }

            return;
        }

        if (shouldBeEnabled && !directionalLight.enabled)
        {
            directionalLight.enabled = true;
        }

        if (!shouldBeEnabled && directionalLight.enabled)
        {
            directionalLight.enabled = false;
        }
    }

    public void OverrideOnDirectionalLights(bool alwaysOn)
    {
        overrideAlwaysOn = alwaysOn;
    }

    private void EnableDirectionalLight() => shouldBeEnabled = true;
    private void DisableDirectionalLight() => shouldBeEnabled = false;

    private void CheckStartVisibility(List<RoomCollider> currentCollidersBlockingView)
    {
        if (controllingColliders.Intersect(currentCollidersBlockingView).Any())
        {
            EnableDirectionalLight();
        }
        else
        {
            DisableDirectionalLight();
        }
    }

    private void CheckEnterVisibility(List<RoomCollider> previousCollidersBlockingView, List<RoomCollider> enterVisibilityColliders)
    {
        if ((controllingColliders.Intersect(previousCollidersBlockingView).Any())) return; //A controllingCollider was already on the previous List

        if (!(controllingColliders.Intersect(enterVisibilityColliders).Any())) return;

        EnableDirectionalLight();
        HandleDirectionalLight();
    }

    private void CheckExitVisibility(List<RoomCollider> previousCollidersBlockingView, List<RoomCollider> currentVisibilityColliders)
    {
        if (!(controllingColliders.Intersect(previousCollidersBlockingView).Any())) return; //There wasn't any controllingCollider on previous list (none o them was bocking view and this was already invisible)
        //At this point, there was at least one collider blocking view

        if (controllingColliders.Intersect(currentVisibilityColliders).Any()) return; //If right now there is at least one collider blocking view, return
        //At this point, there was at least one collider blocking view, but now there is none

        DisableDirectionalLight();
        HandleDirectionalLight();
    }

    private void RoomVisibilityManager_OnStartBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersStartEventArgs e)
    {
        CheckStartVisibility(e.currentRoomVisibilityColliders);
    }

    private void RoomVisibilityManager_OnEnterBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersEnterEventArgs e)
    {
        CheckEnterVisibility(e.previousRoomVisibilityColliders, e.newRoomVisibilityColliders);
    }

    private void RoomVisibilityManager_OnExitBlockingViewColliders(object sender, RoomManager.OnBlockingViewCollidersExitEventArgs e)
    {
        CheckExitVisibility(e.previousRoomVisibilityColliders, e.currentRoomVisibilityColliders);
    }
}
