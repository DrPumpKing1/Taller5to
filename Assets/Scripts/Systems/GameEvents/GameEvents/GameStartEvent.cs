using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStartEvent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float timeForGameStartTrigger;

    public static event EventHandler OnGameStart;

    private void Start()
    {
        StartCoroutine(TriggerGameStart());
    }

    private IEnumerator TriggerGameStart()
    {
        yield return new WaitForSeconds(timeForGameStartTrigger);

        OnGameStart?.Invoke(this, EventArgs.Empty);
    }
}
