using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchElectrodeVisual : MonoBehaviour
{
    public SwitchElectrode switchElectrode;
    public MeshRenderer meshRenderer;

    public Material on;
    public Material off;

    private void LateUpdate()
    {
        meshRenderer.material = (switchElectrode.Power >= Electrode.ACTIVATION_THRESHOLD && switchElectrode.SwitchOn) ? on : off;
    }
}
