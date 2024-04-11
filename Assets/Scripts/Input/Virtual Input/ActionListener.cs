using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionListener : MonoBehaviour
{
    [SerializeField] private string actionName;

    public string ActionName { get { return actionName; } }

    public InputValue value { get; private set; }

    private InputCapturer capturer;

    private void Start()
    {
        value = new InputValue(false, 0, 0f);
    }

    private void LateUpdate()
    {
        ReceiveValue();
    }

    public void ReceiveValue()
    {
        if (capturer == null) return;

        value = capturer.SendInput();
    }

    public void SetCapturer(InputCapturer capturer)
    {
        this.capturer = capturer;
    }

    public void ClearCapturer()
    {
        this.capturer = null;
    }

    public void SetActionName(string actionName)
    {
        this.actionName = actionName;
    }
}
