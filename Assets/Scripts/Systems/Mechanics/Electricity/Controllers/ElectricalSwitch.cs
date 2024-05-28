using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalSwitch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SwitchElectrode switchElectrode;

    [Header("Identifiers")]
    [SerializeField] private int id;

    public int ID => id;
    public SwitchElectrode SwitchElectrode => switchElectrode;

    public void SetSwitchOn() => switchElectrode.InitializeSwitch(true);
    public void SetSwitchOff() => switchElectrode.InitializeSwitch(false);
}
