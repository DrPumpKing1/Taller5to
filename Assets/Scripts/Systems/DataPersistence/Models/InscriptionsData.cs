using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InscriptionsData
{
    public SerializableDictionary<int, bool> inscriptionsTranslated;

    public InscriptionsData()
    {
        inscriptionsTranslated = new SerializableDictionary<int, bool>(); //String -> id; bool -> isTranslated
    }
}
