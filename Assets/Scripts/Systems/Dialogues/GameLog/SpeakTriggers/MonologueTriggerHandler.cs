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
        public string eventCode;
    }

    [Header("Enabler")]
    [SerializeField] private bool enableTriggerMonologues;

    [Header("Monologues")]
    [SerializeField] private MonologueEvent lastMonologueEvent;
    [Space]
    [SerializeField] private float monologueReplacementSafeTime = 1f;

    [Header("Collections")]
    [SerializeField] private List<MonologueEvent> monologues;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private IEnumerator ReadLogMonologueCoroutine(string log)
    {
        if (DialogueManager.Instance.PlayingDialogue()) yield break; //Monologues can't play while another dialogue is playing
        //if (MonologueManager.Instance.PlayingMonologue()) yield break; //Monologues can't play while another monologue is playing

        var compatibleMonologues = monologues.Where(x => x.eventCode == log);

        if (!compatibleMonologues.Any()) yield break;

        MonologueEvent monologueEvent = compatibleMonologues.First();

        if (monologueEvent != null)
        {
            if (monologueEvent.monologue == MonologueManager.Instance.CurrentMonologueSO) yield break;
        }

        if (DialogueManager.Instance.PlayingDialogue())
        {
            DialogueManager.Instance.EndDialogue();
            yield return new WaitForSeconds(monologueReplacementSafeTime);
        }

        if (MonologueManager.Instance.PlayingMonologue())
        {
            MonologueManager.Instance.EndMonologue();
            yield return new WaitForSeconds(monologueReplacementSafeTime);
        }

        MonologueManager.Instance.StartMonologue(monologueEvent.monologue);

        lastMonologueEvent = monologueEvent;
    }

    #region GameLogSubscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (!enableTriggerMonologues) return;

        StartCoroutine(ReadLogMonologueCoroutine(e.gameplayAction.log));
    }
    #endregion
}
