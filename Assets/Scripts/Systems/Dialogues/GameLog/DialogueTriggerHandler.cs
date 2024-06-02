using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueTriggerHandler : MonoBehaviour
{
    [Serializable]
    public class UniqueDialogueEvent
    {
        public int id;
        public DialogueSO dialogue;
        public DialogueSO hint;
        public bool discovered = false;
        public float hintTime;
        public bool end;
        public string eventCode;
    }

    [Header("Enabler")]
    [SerializeField] private bool enableTriggerDialogues;

    [Header("Dialogues")] 
    [SerializeField] private float hintWaitTimer = 0f;
    [SerializeField] private bool waintingHint = false;
    [SerializeField] private UniqueDialogueEvent lastDialogue;
    [Space]
    [SerializeField] private float dialogReplacementSafeTime = 0.5f;

    [Header("Collections")]
    [SerializeField] private List<UniqueDialogueEvent> dialogues;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += ReadLogDialogue;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= ReadLogDialogue;
    }

    private void Update()
    {
        HandleShowHint();
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
        hintWaitTimer = lastDialogue.hintTime;
        DialogueManager.Instance.EndDialogue();

        yield return new WaitForSeconds(dialogReplacementSafeTime);

        DialogueManager.Instance.StartDialogue(lastDialogue.dialogue);
    }

    private void ReadLogDialogue()
    {
        if (!enableTriggerDialogues) return;

        StartCoroutine(ReadLogDialogueCoroutine());
    }

    private IEnumerator ReadLogDialogueCoroutine()
    {
        string lastLog = GameLogManager.Instance.GameLog[^1].log;

        var compatibleDialogues = dialogues.Where(x => x.eventCode == lastLog && !x.discovered);
        
        if(!compatibleDialogues.Any()) yield break;

        UniqueDialogueEvent dialogue = compatibleDialogues.First();
        
        DialogueManager.Instance.EndDialogue();
        MonologueManager.Instance.EndMonologue();

        yield return new WaitForSeconds(dialogReplacementSafeTime);

        DialogueManager.Instance.StartDialogue(dialogue.dialogue);

        dialogue.discovered = true;
        lastDialogue = dialogue;

        if (dialogue.end)
        {
            hintWaitTimer = 0f;
            waintingHint = false;
            yield break;
        }
        
        hintWaitTimer = lastDialogue.hintTime;
        waintingHint = true;
    }
}
