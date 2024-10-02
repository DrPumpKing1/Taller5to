using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossDoor : MonoBehaviour
{
    [Header("Device Settings")]
    [SerializeField] private Transform gateTransform;
    [SerializeField] private Transform restPosition;
    [SerializeField] private Transform powerPosition;
    [SerializeField] private float moveSpeed;
    [SerializeField] private AnimationCurve moveCurve;

    public static event EventHandler OnBossDoorOpen;

    private Coroutine doorMovement;

    private void OnEnable()
    {
        BossStateHandlerOld.OnBossDefeated += BossStateHandler_OnBossDefeated;
    }

    private void OnDisable()
    {
        BossStateHandlerOld.OnBossDefeated -= BossStateHandler_OnBossDefeated;
    }

    private void TriggerMovement(bool state)
    {
        if (doorMovement != null) StopCoroutine(doorMovement);
        doorMovement = StartCoroutine(MoveToPosition(state ? powerPosition.position : restPosition.position));
    }

    private IEnumerator MoveToPosition(Vector3 position)
    {
        Vector3 startPosition = gateTransform.position;
        Vector3 endPosition = position;
        float distance = Vector3.Distance(startPosition, endPosition);
        float time = distance / moveSpeed;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;

            gateTransform.position = Vector3.Lerp(startPosition, endPosition, moveCurve.Evaluate(t / time));

            yield return null;
        }

        gateTransform.position = endPosition;
    }

    private void BossStateHandler_OnBossDefeated(object sender, EventArgs e)
    {
        TriggerMovement(true);
        OnBossDoorOpen?.Invoke(this, EventArgs.Empty);    
    }
}
