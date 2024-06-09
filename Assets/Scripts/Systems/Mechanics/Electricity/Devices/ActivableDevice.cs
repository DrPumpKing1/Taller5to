using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivableDevice : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectableObjectActivation projectableObjectActivation;

    [Header("Electrical Component")]
    [SerializeField] private Electrode electrode;
    [SerializeField] private bool isActive;
    [SerializeField] private bool startActive;

    [Header("Device Control")]
    [SerializeField] protected bool state;

    private bool power => electrode.Power >= Electrode.ACTIVATION_THRESHOLD;
    private bool coherence => state == (power && isActive);
    public bool IsActive => isActive;

    private void OnEnable()
    {
        projectableObjectActivation.OnProjectableObjectActivated += ProjectableObjectActivation_OnProjectableObjectActivated;
    }

    private void OnDisable()
    {
        projectableObjectActivation.OnProjectableObjectActivated -= ProjectableObjectActivation_OnProjectableObjectActivated;
    }

    protected virtual void Start()
    {
        isActive = startActive;
    }

    private void Update()
    {
        if (coherence) return;

        state = power && isActive;
        ToggleActivation();
    }

    public void SetDeviceActive(bool state) => isActive = state;

    protected abstract void ToggleActivation();

    private void ProjectableObjectActivation_OnProjectableObjectActivated(object sender, System.EventArgs e)
    {
        SetDeviceActive(!isActive);
    }
}
