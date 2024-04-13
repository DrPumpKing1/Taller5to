using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwitch : MonoBehaviour
{
    public ElectricalSwitch electricalDevice;
    public MeshRenderer meshRenderer;

    public Material on;
    public Material off;

    private void LateUpdate()
    {
        meshRenderer.material = (electricalDevice.On && electricalDevice.SwitchOn) ? on : off;
    }
}
