using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalElectricalComponentVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ElectricityComponent electricalDevice;
    [SerializeField] private MeshRenderer meshRenderer;

    [Header ("Visual Settings")]
    [SerializeField] private Material on;
    [SerializeField] private Material off;

    private void LateUpdate()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        meshRenderer.material = electricalDevice.On ? on : off;
    }
}
