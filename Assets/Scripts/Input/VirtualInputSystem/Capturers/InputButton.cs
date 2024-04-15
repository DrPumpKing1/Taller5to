using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButton : InputCapturer
{
    private enum ButtonType
    {
        Keyboard,
        MouseLeft,
        MouseRight
    }
    
    [Header("Input")]
    [SerializeField] private ButtonType buttonType;
    public KeyCode buttonKey;

    private enum ButtonMode
    {
        Hold,
        Tap,
        DoubleTap,
        Release
    }

    [Header("Button Mode")]
    [SerializeField] private ButtonMode buttonMode;
    public float tapReleaseInputDurationSeconds;
    public float doubleTapTimeWindowSeconds;
    private float doubleTapTimer;
    private float tapReleaseTimer;

    private bool buttonValue;

    protected override void Update()
    {
        base.Update();

        if(doubleTapTimer > 0) doubleTapTimer -= Time.deltaTime;
        if(tapReleaseTimer > 0) tapReleaseTimer -= Time.deltaTime;
    }

    protected override void GetInput()
    {
        buttonValue = false;

        if( buttonType == ButtonType.Keyboard )
        {
            if (buttonMode == ButtonMode.Hold && Input.GetKey(buttonKey)) buttonValue = true;

            else if (buttonMode == ButtonMode.Tap && Input.GetKeyDown(buttonKey)) tapReleaseTimer = tapReleaseInputDurationSeconds;

            else if (buttonMode == ButtonMode.Release && Input.GetKeyUp(buttonKey)) tapReleaseTimer = tapReleaseInputDurationSeconds;

            else if (buttonMode == ButtonMode.DoubleTap && Input.GetKeyDown(buttonKey))
            {
                if (doubleTapTimer > 0)
                {
                    tapReleaseTimer = tapReleaseInputDurationSeconds;
                    doubleTapTimer = 0f;
                }
                else doubleTapTimer = doubleTapTimeWindowSeconds;
            }

            else if ((buttonMode == ButtonMode.Tap || buttonMode == ButtonMode.Release || buttonMode == ButtonMode.DoubleTap) && tapReleaseTimer > 0) buttonValue = true;
        } 
        
        else
        {
            int mouseButton = buttonType == ButtonType.MouseLeft ? 0 : 1;

            if (buttonMode == ButtonMode.Hold && Input.GetMouseButton(mouseButton)) buttonValue = true;

            else if (buttonMode == ButtonMode.Tap && Input.GetMouseButtonDown(mouseButton)) tapReleaseTimer = tapReleaseInputDurationSeconds;

            else if (buttonMode == ButtonMode.Release && Input.GetMouseButtonUp(mouseButton)) tapReleaseTimer = tapReleaseInputDurationSeconds;

            else if (buttonMode == ButtonMode.DoubleTap && Input.GetMouseButtonDown(mouseButton))
            {
                if (doubleTapTimer > 0)
                {
                    tapReleaseTimer = tapReleaseInputDurationSeconds;
                    doubleTapTimer = 0f;
                }
                else doubleTapTimer = doubleTapTimeWindowSeconds;
            }

            else if ((buttonMode == ButtonMode.Tap || buttonMode == ButtonMode.Release || buttonMode == ButtonMode.DoubleTap) && tapReleaseTimer > 0) buttonValue = true;
        }
    }

    public override InputValue SendInput()
    {
        return new InputValue(buttonValue, buttonValue ? 1 : 0, buttonValue ? 1f : 0f);
    }
}
