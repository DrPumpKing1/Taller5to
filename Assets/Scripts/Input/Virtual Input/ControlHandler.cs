using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandler : MonoBehaviour
{
    public bool connected {  get; private set; }

    [SerializeField] private string controlName;

    public string ControlName { get { return controlName; } }

    [SerializeField] private InputCapturer[] inputs;

    public Dictionary<string, InputCapturer> Inputs;

    private void OnEnable()
    {
        Inputs = new Dictionary<string, InputCapturer>();

        for(int i = 0; i < inputs.Length; i++)
        {
            Inputs.Add(inputs[i].CodeName, inputs[i]);
        }
    }

    public void Connect(bool connected)
    {
        this.connected = connected;
    }
}
