using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListener : MonoBehaviour
{
    private void OnEnable()
    {
        ElectricalSwitchToggle.OnSwitchToggle += ElectricalSwitchToggle_OnSwitchToggle;

        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
        ElectricalDoor.OnDoorPowered += ElectricalDoor_OnDoorPowered;
    }

    private void OnDisable()
    {
        ElectricalSwitchToggle.OnSwitchToggle -= ElectricalSwitchToggle_OnSwitchToggle;

        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
        ElectricalDoor.OnDoorPowered -= ElectricalDoor_OnDoorPowered;
    }

    private void ElectricalSwitchToggle_OnSwitchToggle(object sender, ElectricalSwitchToggle.OnSwitchToggleEventArgs e) => GameLog.Log($"Electrical/ToggleSwitch/{e.switchOn}/{e.id}");

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e) => GameLog.Log($"Worth/ShowDignity/{e.dialect}");
    private void ElectricalDoor_OnDoorPowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e) => GameLog.Log($"Electrical/PowerDoor/{e.id}");


}
