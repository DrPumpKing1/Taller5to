using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScroll : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CameraInput cameraInput;
    [SerializeField] private CinemachineVirtualCamera CMVCam;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSensitivity;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;
    [SerializeField, Range(0f, 1f)] private float startingDistancePercent;
    [SerializeField, Range(0.01f, 0.05f)] private float smoothScrollSpeed;
    [SerializeField] private bool invertScroll;

    public static float orthoSizeRefference;

    public float Distance { get; private set; }
    private float ScrollInput => cameraInput.GetScroll();

    private float desiredDistance;

    private void Awake()
    {
        SetOrthoSizeRefference();
    }

    private void Start()
    {
        InitializeSettings();
    }

    private void LateUpdate()
    {
        HandleDistance();
    }

    private void SetOrthoSizeRefference() => orthoSizeRefference = startingDistancePercent * maxDistance;

    private void InitializeSettings()
    {
        desiredDistance = startingDistancePercent * maxDistance;
        Distance = desiredDistance;
    }

    private void HandleDistance()
    {
        //Handle Inversion
        float processedScrollInput = invertScroll ? -ScrollInput : ScrollInput;

        //Set Distance
        desiredDistance = Mathf.Clamp(desiredDistance - scrollSensitivity * processedScrollInput, minDistance, maxDistance);
        Distance = Mathf.Lerp(Distance, desiredDistance, smoothScrollSpeed);

        CMVCam.m_Lens.OrthographicSize = Distance;   
    }

}   
