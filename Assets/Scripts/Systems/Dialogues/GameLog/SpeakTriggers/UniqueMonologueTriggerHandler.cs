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
        public int id;
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
    [SerializeField] private float monologueReplacementSafeTime = 0.5f;

    [Header("Collections")]
    [SerializeField] private List<UniqueMonologueEvent> uniqueMonologueEvents;

    public List<UniqueMonologueEvent> UniqueMonologueEvents => uniqueMonologueEvents;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += ReadLogMonologue;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= ReadLogMonologue;
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

    private void ReadLogMonologue()
    {
        if (!enableTriggerMonologues) return;

        StartCoroutine(ReadLogMonologueCoroutine());
    }

    private IEnumerator ReadLogMonologueCoroutine()
    {
        //UniqueMonologues may have priority over dialogues

        string lastLog = GameLogManager.Instance.GameLog[^1].log;

        var compatibleMonologues = uniqueMonologueEvents.Where(x => x.eventCode == lastLog && !x.triggered);

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

    public void SetUniqueMonologueTriggered(int id, bool triggered)
    {
        foreach (UniqueMonologueEvent uniqueMonlologueEvent in uniqueMonologueEvents)
        {
            if (id == uniqueMonlologueEvent.id)
            {
                uniqueMonlologueEvent.triggered = triggered;
                return;
            }
        }
    }

    private UniqueMonologueEvent GetUniqueMonologueEventByID(int id)
    {
        foreach (UniqueMonologueEvent uniqueMonologueEvent in uniqueMonologueEvents)
        {
            if (id == uniqueMonologueEvent.id) return uniqueMonologueEvent;
        }

        return null;
    }

    public void SetUniqueMonologuesTriggered(List<int> monoloquesIDs)
    {
        foreach (UniqueMonologueEvent uniqueMonologueEvent in uniqueMonologueEvents)
        {
            uniqueMonologueEvent.triggered = false;
        }

        foreach (int monologueID in monoloquesIDs)
        {
            UniqueMonologueEvent uniqueMonologueTrigger = GetUniqueMonologueEventByID(monologueID);
            if (uniqueMonologueTrigger == null) continue;

            uniqueMonologueTrigger.triggered = true;
        }
    }
}
