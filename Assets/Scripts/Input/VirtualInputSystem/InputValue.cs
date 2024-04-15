using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputValue
{
    private bool logicChannel;
    private int discreteNumericalChannel;
    private float continuousNumericalChannel;

    public bool b { get { return logicChannel; } }
    public int i { get { return discreteNumericalChannel; } }
    public float f { get { return continuousNumericalChannel; } }

    public InputValue(bool logicChannel, int discreteNumericalChannel, float continuousNumericalChannel)
    {
        this.logicChannel = logicChannel;
        this.discreteNumericalChannel = discreteNumericalChannel;
        this.continuousNumericalChannel = continuousNumericalChannel;
    }

    public void SetValue(bool value)
    {
        logicChannel = value;
    }

    public void SetValue(int value)
    {
        discreteNumericalChannel = value;
    }

    public void SetValue(float value)
    {
        continuousNumericalChannel = value;
    }
}
