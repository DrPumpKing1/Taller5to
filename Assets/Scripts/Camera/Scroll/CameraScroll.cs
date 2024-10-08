using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScroll : MonoBehaviour
{
    public static CameraScroll Instance { get; private set; }

    [Header("Enabler")]
    [SerializeField] private bool enableCameraScroll;

    [Header("Components")]
    [SerializeField] private CameraInput cameraInput;
    [SerializeField] private CinemachineVirtualCamera CMVCam;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSensitivity;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;
    [SerializeField, Range(0f, 1f)] private float startingDistancePercent;
    [SerializeField, Range(0.01f, 100f)] private float smoothInputFactor;
    [SerializeField, Range(0.01f, 100f)] private float smoothScrollFactor;
    [SerializeField] private bool invertScroll;

    public float OrthoSizeDefault { get; private set; }
    public float OrthoSizeMax{ get; private set; }
    public float OrthoSizeMin { get; private set; }

    public float Distance { get; private set; }
    private float ScrollInput => cameraInput.GetScroll();

    private float desiredDistance;
    private float smoothInput;

    private float refVelocity;

    public float ScrollFactor { get; private set; }

    private void Awake()
    {
        SetSingleton();
        SetOrthoSizeRefferences();
    }

    private void Start()
    {
        InitializeSettings();
    }

    private void Update()
    {
        HandlePlayerCameraScroll();
    }

    private void LateUpdate()
    {
        ApplyDistance();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CameraScroll instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandlePlayerCameraScroll()
    {
        if (!enableCameraScroll) return;

        if (!CameraFollowHandler.Instance.AllowCameraInputProcessing()) return;

        SmoothInput();
        CalculateDesiredDistance();
        SmoothDistance();
        CalculateCurrentScrollFactor();
    }

    private void SetOrthoSizeRefferences()
    {
        OrthoSizeDefault = startingDistancePercent * maxDistance;
        OrthoSizeMax = maxDistance;
        OrthoSizeMin = minDistance;
    }

    private void InitializeSettings()
    {
        desiredDistance = startingDistancePercent * maxDistance;
        Distance = desiredDistance;
    }

    private void SmoothInput() => smoothInput = Mathf.Lerp(smoothInput, ScrollInput, smoothInputFactor * Time.deltaTime);

    private void CalculateDesiredDistance()
    {
        float processedScrollInput = invertScroll ? -smoothInput : smoothInput;
        desiredDistance = Mathf.Clamp(desiredDistance - scrollSensitivity * processedScrollInput, minDistance, maxDistance);   
    }

    private void SmoothDistance() => Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref refVelocity, smoothScrollFactor * Time.deltaTime);

    private void ApplyDistance() => CMVCam.m_Lens.OrthographicSize = Distance;
    private void CalculateCurrentScrollFactor()
    {
        float orthographicSize = Camera.main.orthographicSize;
        float scrollFactor = Mathf.InverseLerp(OrthoSizeMin, OrthoSizeMax, orthographicSize);
        ScrollFactor = scrollFactor;
    }

    //For CameraFollowHandler
    public void LerpTowardsTargetDistance(float desiredDistance, float smoothFactor) => Distance = Mathf.Lerp(Distance, desiredDistance, smoothFactor * Time.deltaTime);
    public void SetTargetDistance(float desiredDistance) => Distance = desiredDistance;
}   
