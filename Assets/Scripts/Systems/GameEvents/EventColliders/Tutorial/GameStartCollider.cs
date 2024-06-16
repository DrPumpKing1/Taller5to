using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStartCollider : EventCollider
{
    [Header("Settings")]
    [SerializeField] private float timeToTrigger;

    public static event EventHandler OnGameStart;

    protected override void TriggerCollider()
    {
        StartCoroutine(GameStartCoroutine());
    }

    private IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSeconds(timeToTrigger);
        OnGameStart?.Invoke(this, EventArgs.Empty);
    }
}