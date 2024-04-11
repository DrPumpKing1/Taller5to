using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCable : MonoBehaviour
{
    public ElectricityComponent electricalDevice;
    public LineRenderer lineRenderer;

    public Material on;
    public Material off;

    private void LateUpdate()
    {
        lineRenderer.material = electricalDevice.On ? on : off;
    }
}
