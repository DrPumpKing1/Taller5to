using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalSwitch : ElectricityComponent
{
    [SerializeField] private bool switchOn;

    public bool SwitchOn { get { return switchOn; } }

    public override bool CheckValidElectricalContact(ElectricityComponent other)
    {
        if (!switchOn) return false;

        if (!other.source && !other.transmit) return false;

        return base.CheckValidElectricalContact(other);
    }

    public void SetSwitchState(bool swichOn)
    {
        this.switchOn = swichOn;
    }
}
