using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputSwitcher : MonoBehaviour
{
    
    public Dictionary<string, ActionListener> Actions;
    public Dictionary<string, ControlHandler> Controllers;

    [SerializeField] private ControlHandler activeController;

    private void Start()
    {
        UpdateActions();
        UpdateControllers();
        PlugInConnections();
    }

    public void UpdateActions()
    {
        Actions = new Dictionary<string, ActionListener>();

        ActionListener[] actions = GameObject.FindObjectsOfType<ActionListener>();
        foreach (ActionListener action in actions)
        {
            Actions.Add(action.ActionName, action);
        }
    }

    public void UpdateControllers()
    {
        Controllers = new Dictionary<string, ControlHandler>();

        ControlHandler[] controlHandlers = GameObject.FindObjectsOfType<ControlHandler>();
        foreach (ControlHandler controlHandler in controlHandlers)
        {
            Controllers.Add(controlHandler.ControlName, controlHandler);
        }
    }

    public void SetControl(string controlName)
    {
        if (!Controllers.ContainsKey(controlName)) return;

        activeController.Connect(false);

        activeController = Controllers[controlName];

        activeController.Connect(true);

        PlugInConnections();
    }

    private void PlugInConnections()
    {
        if(activeController == null) return;

        foreach(KeyValuePair<string, InputCapturer> keyValuePair in activeController.Inputs)
        {
            if(!Actions.ContainsKey(keyValuePair.Key)) continue;

            Actions[keyValuePair.Key].SetCapturer(keyValuePair.Value);
        }
    }
}
