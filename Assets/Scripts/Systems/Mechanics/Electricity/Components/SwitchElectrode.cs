using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchElectrode : Electrode
{
    [Header("Switch Specific")]
    [SerializeField] private bool switchOn;
    public bool SwitchOn { get { return switchOn; } }

    public override void ReceiveSignal(float intensity)
    {
        if (!switchOn) return;

        base.ReceiveSignal(intensity);
    }

    public void InitializeSwitch(bool switchOn)
    {
        this.switchOn = switchOn;

        if (Electricity.Instance)
        {
            Electricity.Instance.UpdateElectrode(this);
        }
    }


    public void SetSwitch(bool swichOn)
    {
        this.switchOn = swichOn;

        GameLog.Log($"Electrical/ToggleSwitch/{switchOn}");
        
        Electricity.Instance.UpdateElectrode(this);
    }
}
