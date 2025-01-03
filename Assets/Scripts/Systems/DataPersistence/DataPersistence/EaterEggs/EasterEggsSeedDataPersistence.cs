using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggsSeedDataPersistence : MonoBehaviour, IDataPersistence<EasterEggsData>
{
    public void LoadData(EasterEggsData data)
    {
        EasterEggsManager easterEggsManager = FindObjectOfType<EasterEggsManager>();

        easterEggsManager.SetSeed(data.seed);
    }

    public void SaveData(ref EasterEggsData data)
    {
        EasterEggsManager easterEggsManager = FindObjectOfType<EasterEggsManager>();

        data.seed = easterEggsManager.Seed;
    }
}
