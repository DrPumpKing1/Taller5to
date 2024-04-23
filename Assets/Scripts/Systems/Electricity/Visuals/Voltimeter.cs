using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Voltimeter : MonoBehaviour
{
    public Electrode electrode;
    public TextMeshProUGUI textMesh;

    private void LateUpdate()
    {
        textMesh.text = (electrode.Power >= Electrode.ACTIVATION_THRESHOLD) ? $"Voltage: {electrode.Power}" : "0";
    }
}
