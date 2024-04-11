using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlock : MonoBehaviour
{
    public ElectricityComponent electricalDevice;
    public MeshRenderer meshRenderer;

    public Material on;
    public Material off;

    private void LateUpdate()
    {
        meshRenderer.material = electricalDevice.On ? on : off;
    }
}
