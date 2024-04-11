using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAxis : InputCapturer
{
    [Header("Keys")]
    public KeyCode positiveKey;
    public KeyCode negativeKey;

    [Header("Analog")]
    public bool analogInput;
    [Range(0f, 1f)] public float lerpSmoothing;

    private float axisValue;
    private float previousAxisValue;

    protected override void GetInput()
    {
        previousAxisValue = axisValue;

        float differentia = 1;

        if (analogInput)
        {
            differentia = lerpSmoothing * Time.deltaTime;
        } else
        {
            axisValue = 0;
        }

        if (Input.GetKey(positiveKey)) axisValue =  Mathf.Min(axisValue + differentia, 1f);

        if (Input.GetKey(negativeKey)) axisValue = Mathf.Max(axisValue - differentia, -1f);
    }

    public override InputValue SendInput()
    {
        return new InputValue(axisValue > 0f, Mathf.RoundToInt(axisValue), axisValue);
    }
}
