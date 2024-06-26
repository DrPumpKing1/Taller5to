using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WinManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string transitionScene;
    [SerializeField, Range(1f, 10f)] private float timeToTransitioAfterWin;

    public static event EventHandler OnWin;

    private void OnEnable()
    {
        AncientRelic.OnAncientRelicCollected += AncientRelic_OnAncientRelicCollected;
    }

    private void OnDisable()
    {
        AncientRelic.OnAncientRelicCollected -= AncientRelic_OnAncientRelicCollected;
    }

    private void Win()
    {
        StopAllCoroutines();
        StartCoroutine(WinCoroutine());
    }

    private void DeleteAllData()
    {
        PlayerDataPersistenceManager.Instance.DeleteGameData();
        PetDataPersistenceManager.Instance.DeleteGameData();
        ObjectsDataPersistenceManager.Instance.DeleteGameData();
        UIDataPersistenceManager.Instance.DeleteGameData();
        LogDataPersistenceManager.Instance.DeleteGameData();
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(timeToTransitioAfterWin);
        ScenesManager.Instance.FadeLoadTargetScene(transitionScene);

        DeleteAllData();
    }
    private void AncientRelic_OnAncientRelicCollected(object sender, System.EventArgs e)
    {
        Win();

        OnWin?.Invoke(this, EventArgs.Empty);
    }
}
