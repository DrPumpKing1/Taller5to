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

    private void LateUpdate()
    {
        if (!enableCameraScroll) return;

        SmoothInput();
        HandleDistance();

        CalculateCurrentScrollFactor();
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

    private void SmoothInput()
    {
        smoothInput = Mathf.MoveTowards(smoothInput, ScrollInput, smoothInputFactor * Time.deltaTime);
    }

    private void HandleDistance()
    {
        //Handle Inversion
        float processedScrollInput = invertScroll ? -smoothInput : smoothInput;

        //Set Distance
        desiredDistance = Mathf.Clamp(desiredDistance - scrollSensitivity * processedScrollInput, minDistance, maxDistance);
        Distance = Mathf.Lerp(Distance, desiredDistance, smoothScrollFactor * Time.deltaTime);

        CMVCam.m_Lens.OrthographicSize = Distance;   
    }

    private void CalculateCurrentScrollFactor()
    {
        float orthographicSize = Camera.main.orthographicSize;
        float scrollFactor = Mathf.InverseLerp(OrthoSizeMin, OrthoSizeMax, orthographicSize);
        ScrollFactor = scrollFactor;
    }

}   
