using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InscriptionsTranslatedPersistenceManager : DataPersistenceManager<InscriptionsData>
{
    public static InscriptionsTranslatedPersistenceManager Instance { get; private set; }

    protected override void SetSingleton()
    {
        if (Instance != null)
        {
            Debug.LogError($"There is more than one InscriptionsTranslatedPersistenceManager Instance");
        }

        Instance = this;
    }
}
