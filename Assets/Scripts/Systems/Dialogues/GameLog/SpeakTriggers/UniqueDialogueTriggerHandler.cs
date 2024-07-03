using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniqueDialogueTriggerHandler : MonoBehaviour
{
    public static UniqueDialogueTriggerHandler Instance { get; private set; }

    [Serializable]
    public class UniqueDialogueEvent
    {
        public int id;
        public DialogueSO dialogue;
        public DialogueSO hint;
        public bool triggered;
        public float hintTime;
        public bool end;
        public string eventCode;
    }

    [Header("Enabler")]
    [SerializeField] private bool enableTriggerDialogues;

    [Header("Dialogues")]
    [SerializeField] private float hintWaitTimer = 0f;
    [SerializeField] private bool waintingHint = false;
    [SerializeField] private UniqueDialogueEvent lastDialogueEvent;
    [Space]
    [SerializeField] private float dialogReplacementSafeTime = 0.5f;

    [Header("Collections")]
    [SerializeField] private List<UniqueDialogueEvent> uniqueDialogueEvents;

    public List<UniqueDialogueEvent> UniqueDialogueEvents => uniqueDialogueEvents;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += ReadLogDialogue;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= ReadLogDialogue;
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
            Debug.LogWarning("There is more than one UniqueDialogueTriggerHandler instance, proceding to destroy duplicate");
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
        hintWaitTimer = lastDialogueEvent.hintTime;
        DialogueManager.Instance.EndDialogue();

        yield return new WaitForSeconds(dialogReplacementSafeTime);

        DialogueManager.Instance.StartDialogue(lastDialogueEvent.dialogue);
    }

    private void ReadLogDialogue()
    {
        if (!enableTriggerDialogues) return;

        StartCoroutine(ReadLogDialogueCoroutine());
    }

    private IEnumerator ReadLogDialogueCoroutine()
    {
        string lastLog = GameLogManager.Instance.GameLog[^1].log;

        var compatibleDialogues = uniqueDialogueEvents.Where(x => x.eventCode == lastLog && !x.triggered);

        if (!compatibleDialogues.Any()) yield break;

        UniqueDialogueEvent dialogueEvent = compatibleDialogues.First();

        if (MonologueManager.Instance.PlayingMonologue())
        {
            MonologueManager.Instance.EndMonologue();
            yield return new WaitForSeconds(dialogReplacementSafeTime);
        }

        if (DialogueManager.Instance.PlayingDialogue())
        {
            DialogueManager.Instance.EndDialogue();
            yield return new WaitForSeconds(dialogReplacementSafeTime);
        }

        DialogueManager.Instance.StartDialogue(dialogueEvent.dialogue);

        dialogueEvent.triggered = true;
        lastDialogueEvent = dialogueEvent;

        if (dialogueEvent.end)
        {
            hintWaitTimer = 0f;
            waintingHint = false;
            yield break;
        }

        hintWaitTimer = lastDialogueEvent.hintTime;
        waintingHint = true;
    }

    public void SetUniqueDialogueTriggered(int id, bool triggered)
    {
        foreach (UniqueDialogueEvent uniqueDialogueEvent in uniqueDialogueEvents)
        {
            if (id == uniqueDialogueEvent.id)
            {
                uniqueDialogueEvent.triggered = triggered;
                return;
            }
        }
    }

    private UniqueDialogueEvent GetUniqueDialoqueEventByID(int id)
    {
        foreach (UniqueDialogueEvent uniqueDialogueEvent in uniqueDialogueEvents)
        {
            if (id == uniqueDialogueEvent.id) return uniqueDialogueEvent;
        }

        return null;
    }

    public void SetUniqueDialoguesTriggered(List<int> dialoguesIDs)
    {
        foreach (UniqueDialogueEvent uniqueDialogueEvent in uniqueDialogueEvents)
        {
            uniqueDialogueEvent.triggered = false;
        }

        foreach (int dialogueID in dialoguesIDs)
        {
            UniqueDialogueEvent uniqueDialogueEvent = GetUniqueDialoqueEventByID(dialogueID);
            if (uniqueDialogueEvent == null) continue;

            uniqueDialogueEvent.triggered = true;
        }
    }
}
