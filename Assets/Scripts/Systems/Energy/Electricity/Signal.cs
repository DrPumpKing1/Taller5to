using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal
{
    public Vector3 origin {  get; private set; }
    public float intensity { get; private set; }

    public Signal(Vector3 origin, float intensity)
    {
        this.origin = origin;
        this.intensity = intensity;
    }
}
