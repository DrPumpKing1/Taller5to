using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableElectrodeVisual : MonoBehaviour
{
    public Electrode electrode;
    public LineRenderer lineRenderer;

    public Material on;
    public Material off;

    private void LateUpdate()
    {
        lineRenderer.material = electrode.Power >= Electrode.ACTIVATION_THRESHOLD ? on : off;
    }
}
