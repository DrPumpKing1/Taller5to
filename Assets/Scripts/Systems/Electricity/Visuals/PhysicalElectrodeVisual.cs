using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalElectrodeVisual : MonoBehaviour
{
    public Electrode electrode;
    public MeshRenderer meshRenderer;

    public Material on;
    public Material off;

    private void LateUpdate()
    {
        meshRenderer.material = electrode.Power >= Electrode.ACTIVATION_THRESHOLD ? on : off;
    }
}
