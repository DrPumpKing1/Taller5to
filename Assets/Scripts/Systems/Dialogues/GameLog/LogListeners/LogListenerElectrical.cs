using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerElectrical : MonoBehaviour
{
    private void OnEnable()
    {
        //ELECTRICAL
        ElectricalSwitchToggle.OnSwitchToggle += ElectricalSwitchToggle_OnSwitchToggle;
        ElectricalDoor.OnDoorPowered += ElectricalDoor_OnDoorPowered;
        InscriptionPowering.OnInscriptionPoweringFirstTime += InscriptionPowering_OnInscriptionPoweringFirstTime;
    }

    private void OnDisable()
    {
        //ELECTRICAL
        ElectricalSwitchToggle.OnSwitchToggle -= ElectricalSwitchToggle_OnSwitchToggle;
        ElectricalDoor.OnDoorPowered -= ElectricalDoor_OnDoorPowered;
        InscriptionPowering.OnInscriptionPoweringFirstTime -= InscriptionPowering_OnInscriptionPoweringFirstTime;
    }

    private void ElectricalSwitchToggle_OnSwitchToggle(object sender, ElectricalSwitchToggle.OnSwitchToggleEventArgs e) => GameLogManager.Instance.Log($"Electrical/ToggleSwitch/{e.switchOn}/{e.id}");
    private void ElectricalDoor_OnDoorPowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerDoor/{e.id}");
    private void InscriptionPowering_OnInscriptionPoweringFirstTime(object sender, InscriptionPowering.OnInscriptionPoweringEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerInscription/{e.id}");
}


