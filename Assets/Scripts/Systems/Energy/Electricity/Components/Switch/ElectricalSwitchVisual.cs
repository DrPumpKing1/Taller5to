using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalSwitchVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ElectricalSwitch electricalDevice;
    [SerializeField] private MeshRenderer meshRenderer;

    [Header("Visual Settings")]
    [SerializeField] private Material on;
    [SerializeField] private Material off;

    private void LateUpdate()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        meshRenderer.material = (electricalDevice.On && electricalDevice.SwitchOn) ? on : off;
    }
}
