using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EasterEggsObjectsHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<EasterEggsSeedObject> easterEggsSeedObjects;

    [Header("Settings")]//Filled in runtime
    [SerializeField] private List<EasterEggsSeedObject> enabledEasterEggsSeedObjects;

    [System.Serializable]
    public class EasterEggsSeedObject
    {
        [Range(1, 1000)] public int minSeedNumber;
        [Range(1, 1000)] public int maxSeedNumber;
        public List<GameObject> seedGameObject;
    }

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

    private void CheckEnableEasterEggsObjectsBySeed(int seed)
    {
        foreach (EasterEggsSeedObject easterEggsSeedObject in easterEggsSeedObjects)
        {
            if (easterEggsSeedObject.minSeedNumber > seed)
            {
                SetAllGameObjectsInSeed(easterEggsSeedObject, false);
                continue;
            }
                
            if (easterEggsSeedObject.maxSeedNumber < seed)
            {
                SetAllGameObjectsInSeed(easterEggsSeedObject, false);
                continue;
            }

            SetAllGameObjectsInSeed(easterEggsSeedObject, true);
            AddEasterEggsSeedObjectToEnabledList(easterEggsSeedObject);
        }
    }

    private EasterEggsSeedObject GetEasterEggsSeedObjectBySeed(int seed)
    {
        foreach(EasterEggsSeedObject easterEggsSeedObject in easterEggsSeedObjects)
        {
            if (easterEggsSeedObject.minSeedNumber < seed) continue;
            if (easterEggsSeedObject.maxSeedNumber > seed) continue;

            return easterEggsSeedObject;
        }

        return null;
    }

    private void SetAllGameObjectsInSeed(EasterEggsSeedObject seedObject, bool active)
    {
        foreach(GameObject seedGameObject in seedObject.seedGameObject)
        {
            if(active) seedGameObject.SetActive(true);
            else seedGameObject.SetActive(false);
        }
    }

    private void AddEasterEggsSeedObjectToEnabledList(EasterEggsSeedObject easterEggsSeedObject) => enabledEasterEggsSeedObjects.Add(easterEggsSeedObject);

    #region  EasterEggsManager Subscriptions
    private void EasterEggsManager_OnSeedGenerated(object sender, EasterEggsManager.OnSeedEventArgs e)
    {
        CheckEnableEasterEggsObjectsBySeed(e.seed);
    }

    private void EasterEggsManager_OnSeedSet(object sender, EasterEggsManager.OnSeedEventArgs e)
    {
        CheckEnableEasterEggsObjectsBySeed(e.seed);
    }
    #endregion
}
