using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectsData
{
    public SerializableDictionary<int, bool> switchesToggled;

    public ObjectsData()
    {
        switchesToggled = new SerializableDictionary<int, bool>(); //string -> id; bool -> isOn;
    }
}
