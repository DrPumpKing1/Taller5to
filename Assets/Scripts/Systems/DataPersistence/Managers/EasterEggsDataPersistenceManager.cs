using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggsDataPersistenceManager : DataPersistenceManager<EasterEggsData>
{
    public static EasterEggsDataPersistenceManager Instance { get; private set; }

    private void OnEnable()
    {
        EasterEggsManager.OnSeedGenerated += EasterEggsManager_OnSeedGenerated;
    }

    private void OnDisable()
    {
        EasterEggsManager.OnSeedGenerated -= EasterEggsManager_OnSeedGenerated;
    }

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one EasterEggsDataPersistenceManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    #region EasterEggsManager Subscriptions
    private void EasterEggsManager_OnSeedGenerated(object sender, EasterEggsManager.OnSeedEventArgs e)
    {
        SaveGameData();
    }
    #endregion
}

