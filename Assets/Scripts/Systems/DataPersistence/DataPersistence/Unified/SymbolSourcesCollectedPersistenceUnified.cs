using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSourcesCollectedPersistenceUnified : MonoBehaviour, IDataPersistence<GameData>
{
    public void LoadData(GameData data)
    {
        DialectSymbolSource[] dialectSymbolSources = FindObjectsOfType<DialectSymbolSource>();

        foreach (DialectSymbolSource dialectSymbolSource in dialectSymbolSources)
        {
            foreach (KeyValuePair<int, bool> symbolSourceCollected in data.symbolSourcesCollected)
            {
                if (dialectSymbolSource.ID == symbolSourceCollected.Key)
                {
                    if (symbolSourceCollected.Value) dialectSymbolSource.SetIsCollected();
                    break;
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        DialectSymbolSource[] dialectSymbolSources = FindObjectsOfType<DialectSymbolSource>();

        foreach (DialectSymbolSource dialectSymbolSource in dialectSymbolSources)
        {
            if (data.symbolSourcesCollected.ContainsKey(dialectSymbolSource.ID)) data.symbolSourcesCollected.Remove(dialectSymbolSource.ID);
        }

        foreach (DialectSymbolSource dialectSymbolSource in dialectSymbolSources)
        {
            bool collected = dialectSymbolSource.IsCollected;

            data.symbolSourcesCollected.Add(dialectSymbolSource.ID, collected);
        }
    }
}
