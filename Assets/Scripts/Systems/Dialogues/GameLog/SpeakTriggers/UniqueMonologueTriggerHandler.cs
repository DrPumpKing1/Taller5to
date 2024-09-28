using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniqueMonologueTriggerHandler : MonoBehaviour
{
    public static UniqueMonologueTriggerHandler Instance { get; private set; }

    [Serializable]
    public class UniqueMonologueEvent
    {
        public MonologueSO monologue;
        public MonologueSO hint;
        public bool triggered;
        public float hintTime;
        public bool end;
        public string eventCode;
    }

    [Header("Enabler")]
    [SerializeField] private bool enableTriggerMonologues;

    [Header("Monologues")]
    [SerializeField] private float hintWaitTimer = 0f;
    [SerializeField] private bool waintingHint = false;
    [SerializeField] private UniqueMonologueEvent lastMonologueEvent;
    [Space]
    [SerializeField] private float monologueReplacementSafeTime = 1f;

    [Header("Collections")]
    [SerializeField] private List<UniqueMonologueEvent> uniqueMonologueEvents;

    public List<UniqueMonologueEvent> UniqueMonologueEvents => uniqueMonologueEvents;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Update()
    {
        HandleShowHint();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one UniqueMonologueTriggerHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void HandleShowHint()
    {
        if (waintingHint)
        {
            if (hintWaitTimer > 0f) hintWaitTimer -= Time.deltaTime;

            else
            {
                StartCoroutine(ShowHint());
            }
        }
    }

    private IEnumerator ShowHint()
    {
        hintWaitTimer = lastMonologueEvent.hintTime;
        MonologueManager.Instance.EndMonologue();

        yield return new WaitForSeconds(monologueReplacementSafeTime);

        MonologueManager.Instance.StartMonologue(lastMonologueEvent.monologue);
    }

    private IEnumerator ReadLogMonologueCoroutine(string log)
    {
        //UniqueMonologues may have priority over dialogues

        var compatibleMonologues = uniqueMonologueEvents.Where(x => x.eventCode == log && !x.triggered);

        if (!compatibleMonologues.Any()) yield break;

        UniqueMonologueEvent monologueEvent = compatibleMonologues.First();

        if (MonologueManager.Instance.PlayingMonologue())
        {
            MonologueManager.Instance.EndMonologue();
            yield return new WaitForSeconds(monologueReplacementSafeTime);
        }

        if (DialogueManager.Instance.PlayingDialogue())
        {
            DialogueManager.Instance.EndDialogue();
            yield return new WaitForSeconds(monologueReplacementSafeTime);
        }

        MonologueManager.Instance.StartMonologue(monologueEvent.monologue);

        monologueEvent.triggered = true;
        lastMonologueEvent = monologueEvent;

        if (monologueEvent.end)
        {
            hintWaitTimer = 0f;
            waintingHint = false;
            yield break;
        }

        hintWaitTimer = lastMonologueEvent.hintTime;
        waintingHint = true;
    }

    public void SetUniqueMonologueTriggered(int indexInList, bool triggered)
    {
        for (int i = 0; i < uniqueMonologueEvents.Count; i++)
        {
            if (indexInList == i) uniqueMonologueEvents[i].triggered = triggered;
        }
    }

    private UniqueMonologueEvent GetUniqueMonologueEventByIndex(int indexInList)
    {
        for (int i = 0; i < uniqueMonologueEvents.Count; i++)
        {
            if (indexInList == i) return uniqueMonologueEvents[i];
        }

        return null;
    }

    private UniqueMonologueEvent GetUniqueMonologueEventByMonologueSO(MonologueSO monologueSO)
    {
        foreach (UniqueMonologueEvent uniqueMonologueEvent in uniqueMonologueEvents)
        {
            if (uniqueMonologueEvent.monologue == monologueSO) return uniqueMonologueEvent;
        }

        return null;
    }

    public void ReplaceUniqueMonologuesTriggered(List<MonologueSO> monologueSOs, bool triggered)
    {
        foreach (UniqueMonologueEvent uniqueMonologueEvent in uniqueMonologueEvents)
        {
            uniqueMonologueEvent.triggered = false;
        }

        foreach (MonologueSO monologueSO in monologueSOs)
        {
            UniqueMonologueEvent uniqueMonologueTrigger = GetUniqueMonologueEventByMonologueSO(monologueSO);
            if (uniqueMonologueTrigger == null) continue;

            uniqueMonologueTrigger.triggered = triggered;
        }
    }

    #region GameLog Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (!enableTriggerMonologues) return;

        StartCoroutine(ReadLogMonologueCoroutine(e.gameplayAction.log));
    }
    #endregion
}
