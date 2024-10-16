using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerElectrical : MonoBehaviour
{
    private void OnEnable()
    {
        //ELECTRICAL
        ElectricalSwitchToggle.OnSwitchToggle += ElectricalSwitchToggle_OnSwitchToggle;

        ElectricalDoor.OnDoorPower += ElectricalDoor_OnDoorPowered;
        ElectricalDoor.OnDoorDePowered += ElectricalDoor_OnDoorDePowered;

        InscriptionPowering.OnInscriptionPoweringFirstTime += InscriptionPowering_OnInscriptionPoweringFirstTime;
        InscriptionPowering.OnInscriptionPower += InscriptionPowering_OnInscriptionPower;
        InscriptionPowering.OnInscriptionDePower += InscriptionPowering_OnInscriptionDePower;

        ElectricalDrawbridge.OnDrawbridgePower += ElectricalDrawbridge_OnDrawbridgePower;
        ElectricalDrawbridge.OnDrawbridgeDePower += ElectricalDrawbridge_OnDrawbridgeDePower;

        ElectricalExtensibleBridgeOld.OnExtensibleBridgePower += ElectricalExtensibleBridge_OnExtensibleBridgePower;
        ElectricalExtensibleBridgeOld.OnExtensibleBridgeDePower += ElectricalExtensibleBridge_OnExtensibleBridgeDePower;

        HiddenSourceReceiver.OnAnyHiddenSourceReceiverPower += HiddenSourceReceiver_OnAnyHiddenSourceReceiverPower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverDePower += HiddenSourceReceiver_OnAnyHiddenSourceReceiverDePower;

        AncientRelicShield.OnAncientRelicShieldDepowered += AncientRelicDoor_OnAncientRelicShieldDePower;
    }

    private void OnDisable()
    {
        //ELECTRICAL
        ElectricalSwitchToggle.OnSwitchToggle -= ElectricalSwitchToggle_OnSwitchToggle;

        ElectricalDoor.OnDoorPower -= ElectricalDoor_OnDoorPowered;
        ElectricalDoor.OnDoorDePowered -= ElectricalDoor_OnDoorDePowered;

        InscriptionPowering.OnInscriptionPoweringFirstTime -= InscriptionPowering_OnInscriptionPoweringFirstTime;
        InscriptionPowering.OnInscriptionPower -= InscriptionPowering_OnInscriptionPower;
        InscriptionPowering.OnInscriptionDePower -= InscriptionPowering_OnInscriptionDePower;

        ElectricalDrawbridge.OnDrawbridgePower -= ElectricalDrawbridge_OnDrawbridgePower;
        ElectricalDrawbridge.OnDrawbridgeDePower -= ElectricalDrawbridge_OnDrawbridgeDePower;

        ElectricalExtensibleBridgeOld.OnExtensibleBridgePower -= ElectricalExtensibleBridge_OnExtensibleBridgePower;
        ElectricalExtensibleBridgeOld.OnExtensibleBridgeDePower -= ElectricalExtensibleBridge_OnExtensibleBridgeDePower;

        HiddenSourceReceiver.OnAnyHiddenSourceReceiverPower -= HiddenSourceReceiver_OnAnyHiddenSourceReceiverPower;
        HiddenSourceReceiver.OnAnyHiddenSourceReceiverDePower -= HiddenSourceReceiver_OnAnyHiddenSourceReceiverDePower;

        AncientRelicShield.OnAncientRelicShieldDepowered -= AncientRelicDoor_OnAncientRelicShieldDePower;
    }



    private void ElectricalSwitchToggle_OnSwitchToggle(object sender, ElectricalSwitchToggle.OnSwitchToggleEventArgs e) => GameLogManager.Instance.Log($"Electrical/ToggleSwitch/{e.switchOn}/{e.id}");

    private void ElectricalDoor_OnDoorPowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerDoor/{e.id}");
    private void ElectricalDoor_OnDoorDePowered(object sender, ElectricalDoor.OnDoowPoweredEventArgs e) => GameLogManager.Instance.Log($"Electrical/DePowerDoor/{e.id}");

    private void InscriptionPowering_OnInscriptionPoweringFirstTime(object sender, InscriptionPowering.OnInscriptionPoweringEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerInscriptionFirstTime/{e.id}");
    private void InscriptionPowering_OnInscriptionPower(object sender, InscriptionPowering.OnInscriptionPoweringEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerInscription/{e.id}");
    private void InscriptionPowering_OnInscriptionDePower(object sender, InscriptionPowering.OnInscriptionPoweringEventArgs e) => GameLogManager.Instance.Log($"Electrical/DePowerInscription/{e.id}");

    private void ElectricalDrawbridge_OnDrawbridgePower(object sender, ElectricalDrawbridge.OnDrawbridgePoweredEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerDrawbridge/{e.id}");
    private void ElectricalDrawbridge_OnDrawbridgeDePower(object sender, ElectricalDrawbridge.OnDrawbridgePoweredEventArgs e) => GameLogManager.Instance.Log($"Electrical/DePowerDrawbridge/{e.id}");

    private void ElectricalExtensibleBridge_OnExtensibleBridgePower(object sender, ElectricalExtensibleBridgeOld.OnExtensibleBridgePowerEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerExtensibleBridge/{e.id}");
    private void ElectricalExtensibleBridge_OnExtensibleBridgeDePower(object sender, ElectricalExtensibleBridgeOld.OnExtensibleBridgePowerEventArgs e) => GameLogManager.Instance.Log($"Electrical/DePowerExtensibleBridge/{e.id}");

    private void HiddenSourceReceiver_OnAnyHiddenSourceReceiverPower(object sender, HiddenSourceReceiver.OnHiddenSourcePowerEventArgs e) => GameLogManager.Instance.Log($"Electrical/PowerReceiver/{e.id}");
    private void HiddenSourceReceiver_OnAnyHiddenSourceReceiverDePower(object sender, HiddenSourceReceiver.OnHiddenSourcePowerEventArgs e) => GameLogManager.Instance.Log($"Electrical/DePowerReceiver/{e.id}");

    private void AncientRelicDoor_OnAncientRelicShieldDePower(object sender, System.EventArgs e) => GameLogManager.Instance.Log($"Electrical/DePowerAncientRelicShield");
}


