using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectButtonRelease : InputCapturer
{
    [Header("Input")]
    public KeyCode buttonKey;
    public float cooldown;
    public int frames;

    private float timer;

    private bool buttonValue;

    protected override void Update()
    {
        base.Update();
        if (timer > 0) timer -= Time.deltaTime;
    }

    protected override void GetInput()
    {
        if (Input.GetKeyUp(buttonKey) && timer <= 0)
        {
            timer = cooldown;

            StartCoroutine(DeactivateOneFrameAfter());
        }
    }

    private IEnumerator DeactivateOneFrameAfter()
    {
        buttonValue = true;

        for (int i = 0; i < frames; i++)
        {
            yield return null;
        }

        buttonValue = false;
    }

    public override InputValue SendInput()
    {
        return new InputValue(buttonValue, buttonValue ? 1 : 0, buttonValue ? 1f : 0f);
    }
}
