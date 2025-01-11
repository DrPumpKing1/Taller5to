using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomsData
{
    public SerializableDictionary<int, bool> narrativeRoomsVisited;

    public RoomsData()
    {
        narrativeRoomsVisited = new SerializableDictionary<int, bool>(); //string -> id; bool -> visited;
    }
}
