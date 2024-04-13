using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ElectricityComponent electricalDevice;
    [SerializeField] private LineRenderer lineRenderer;

    [Header("Visual Settings")]
    [SerializeField] private Material on;
    [SerializeField] private Material off;

    private void LateUpdate()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        lineRenderer.material = electricalDevice.On ? on : off;
    }
}
