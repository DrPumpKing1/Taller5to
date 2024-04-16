using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class UICamera : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera mainCamera;

    private Camera _UICamera;

    private void Awake()
    {
        _UICamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        UpdateUICameraOrthoSize();
    }

    private void UpdateUICameraOrthoSize() => _UICamera.orthographicSize = mainCamera.orthographicSize;

}
