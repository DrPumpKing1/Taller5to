using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesToggledPersistence : MonoBehaviour, IDataPersistence<ObjectsData>
{
    public void LoadData(ObjectsData data)
    {
        ElectricalSwitch[] electricalSwitches = FindObjectsOfType<ElectricalSwitch>();

        foreach (ElectricalSwitch electricalSwitch in electricalSwitches)
        {
            foreach (KeyValuePair<int, bool> switchToggle in data.switchesToggled)
            {
                if (electricalSwitch.ID == 0) continue;

                if (electricalSwitch.ID == switchToggle.Key)
                {
                    if (switchToggle.Value) electricalSwitch.InitializeSwitch(true);
                    else electricalSwitch.InitializeSwitch(false);
                    break;
                }
            }
        }
    }

    public void SaveData(ref ObjectsData data)
    {
        ElectricalSwitch[] electricalSwitches = FindObjectsOfType<ElectricalSwitch>();

        foreach (ElectricalSwitch electricalSwitch in electricalSwitches)
        {
            if (data.switchesToggled.ContainsKey(electricalSwitch.ID)) data.switchesToggled.Remove(electricalSwitch.ID);
        }

        foreach (ElectricalSwitch electricalSwitch in electricalSwitches)
        {
            if (electricalSwitch.ID == 0) continue;

            bool isOn = electricalSwitch.IsOn;
            data.switchesToggled.Add(electricalSwitch.ID, isOn);
        }
    }
}
