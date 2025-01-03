using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggsRoomHandler : MonoBehaviour
{
    private void OnEnable()
    {
        EasterEggsManager.OnSeedGenerated += EasterEggsManager_OnSeedGenerated;
        EasterEggsManager.OnSeedSet += EasterEggsManager_OnSeedSet;
    }

    private void OnDisable()
    {
        EasterEggsManager.OnSeedGenerated -= EasterEggsManager_OnSeedGenerated;
        EasterEggsManager.OnSeedSet -= EasterEggsManager_OnSeedSet;
    }

    #region  EasterEggsManager Subscriptions
    private void EasterEggsManager_OnSeedGenerated(object sender, EasterEggsManager.OnSeedEventArgs e)
    {
        Debug.Log($"Seed Generated: {e.seed}");
    }

    private void EasterEggsManager_OnSeedSet(object sender, EasterEggsManager.OnSeedEventArgs e)
    {
        Debug.Log($"Seed Set: {e.seed}");
    }
    #endregion
}
