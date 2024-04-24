using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Signal 
{
    public float intensity;
    public AnimationCurve powerCurve;
    public float duration;

    public float GetPower(float timer)
    {
        timer = Mathf.Clamp(timer, 0f, duration);

        return intensity * powerCurve.Evaluate(timer);
    }

    public Signal(float intensity, AnimationCurve powerCurve)
    {
        this.intensity = intensity;
        this.powerCurve = powerCurve;
        this.duration = Electrode.ELECTRICITY_TICK_DURATION;
    }
}
