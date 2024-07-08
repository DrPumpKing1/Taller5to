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

    private IEnumerator ReadLogDialogueCoroutine(string log)
    {
        var compatibleDialogues = uniqueDialogueEvents.Where(x => x.eventCode == log && !x.triggered);

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

    public void SetUniqueDialogueTriggered(int indexInList, bool triggered)
    {
        for(int i=0; i<uniqueDialogueEvents.Count; i++)
        {
            if(indexInList == i) uniqueDialogueEvents[i].triggered = triggered;
        }
    }

    private UniqueDialogueEvent GetUniqueDialoqueEventByIndex(int indexInList)
    {
        for (int i = 0; i < uniqueDialogueEvents.Count; i++)
        {
            if (indexInList == i) return uniqueDialogueEvents[i];
        }

        return null;
    }

    private UniqueDialogueEvent GetUniqueDialoqueEventByDialogueSO(DialogueSO dialogueSO)
    {
        foreach (UniqueDialogueEvent uniqueDialogueEvent in uniqueDialogueEvents)
        {
            if (uniqueDialogueEvent.dialogue == dialogueSO) return uniqueDialogueEvent;
        }

        return null;
    }
    

    public void ReplaceUniqueDialoguesTriggered(List<DialogueSO> dialogueSOs, bool triggered)
    {
        foreach (UniqueDialogueEvent uniqueDialogueEvent in uniqueDialogueEvents)
        {
            uniqueDialogueEvent.triggered = false;
        }

        foreach (DialogueSO dialogueSO in dialogueSOs)
        {
            UniqueDialogueEvent uniqueDialogueEvent = GetUniqueDialoqueEventByDialogueSO(dialogueSO);
            if (uniqueDialogueEvent == null) continue;

            uniqueDialogueEvent.triggered = triggered;
        }
    }

    #region GameLog Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (!enableTriggerDialogues) return;

        StartCoroutine(ReadLogDialogueCoroutine(e.gameplayAction.log));
    }
    #endregion
}
