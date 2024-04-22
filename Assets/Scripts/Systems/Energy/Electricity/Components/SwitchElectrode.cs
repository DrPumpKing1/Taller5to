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
        if (!switchOn)
        {
            signal = new Signal(0, powerCurve);
            powerTimer = 0;
            return;
        }

        base.ReceiveSignal(intensity);
    }

    public void SetSwitch(bool swichOn)
    {
        this.switchOn = swichOn;

        Electricity.Instance.UpdateElectrode(this);
    }
}
