using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchElectrode : Electrode
{
    [Header("Switch Specific")]
    [SerializeField] private bool switchOn;
    public bool SwitchOn { get { return switchOn; } }

    public override void ReceiveSignal(Signal signal)
    {
        if (!switchOn)
        {
            power = 0;
            return;
        }

        base.ReceiveSignal(signal);
    }

    public void SetSwitch(bool swichOn)
    {
        this.switchOn = swichOn;

        Electricity.Instance.UpdateElectrode(this);
    }
}
