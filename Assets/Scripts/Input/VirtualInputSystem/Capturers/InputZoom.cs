using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputZoom : InputCapturer
{
    [Header("Input")]
    public float scrollSensitivity;
    public bool invertScroll;

    private float scrollValue;

    protected override void GetInput()
    {
        scrollValue = (invertScroll ? -1 : 1) * Input.mouseScrollDelta.y;
    }

    public override InputValue SendInput()
    {
        return new InputValue(scrollValue != 0, Mathf.RoundToInt(scrollValue), scrollValue);
    }
}
