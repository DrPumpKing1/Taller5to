using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonologueTriggerHandler : MonoBehaviour
{
    [Serializable]
    public class MonologueEvent
    {
        public int id;
        public MonologueSO monologue;
        public float hintTime;
        public bool end;
        public string eventCode;
    }

    [Header("Enabler")]
    [SerializeField] private bool enableTriggerMonologues;

    [Header("Monologues")]
    [SerializeField] private MonologueEvent lastMonologueEvent;
    [Space]
    [SerializeField] private float monologueReplacementSafeTime = 0.5f;

    [Header("Collections")]
    [SerializeField] private List<MonologueEvent> monologues;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += ReadLogMonologue;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= ReadLogMonologue;
    }

    private void ReadLogMonologue()
    {
        if (!enableTriggerMonologues) return;

        StartCoroutine(ReadLogMonologueCoroutine());
    }

    private IEnumerator ReadLogMonologueCoroutine()
    {
        if (DialogueManager.Instance.PlayingDialogue()) yield break; //Dialogues have priority over monologues

        string lastLog = GameLogManager.Instance.GameLog[^1].log;

        var compatibleMonologues = monologues.Where(x => x.eventCode == lastLog);

        if (!compatibleMonologues.Any()) yield break;

        MonologueEvent monologueEvent = compatibleMonologues.First();

        DialogueManager.Instance.EndDialogue();
        MonologueManager.Instance.EndMonologue();

        yield return new WaitForSeconds(monologueReplacementSafeTime);

        MonologueManager.Instance.StartMonologue(monologueEvent.monologue);

        lastMonologueEvent = monologueEvent;
    }
}
