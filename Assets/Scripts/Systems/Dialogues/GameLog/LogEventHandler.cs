using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogEventHandler : MonoBehaviour
{
    [Serializable]
    public class UniqueDialogueEvent
    {
        public DialogueSO dialogue;
        public DialogueSO hint;
        public bool discovered = false;
        public float hintTime;
        public bool end;
        public string eventCode;
    }

    [Serializable]
    public class MonologueReaction
    {
        public MonologueSO monologue;
        public float reactionTime;
        public List<string> logPattern;
    }
    
    [Header("Dialogues")] 
    [SerializeField] private float hintWaitTimer = 0f;
    [SerializeField] private bool waintingHint = false;
    [SerializeField] private UniqueDialogueEvent lastDialogue;

    [Header("Monologues")] 
    [SerializeField] private float reactionCooldown;
    [SerializeField] private float reactionTimer;

    [Header("Collections")]
    [SerializeField] private List<UniqueDialogueEvent> dialogues;
    [SerializeField] private List<MonologueReaction> monologues;
    private void OnEnable()
    {
        GameLog.Instance.OnLogAdd += () => StartCoroutine(ReadLogDialogue());
        GameLog.Instance.OnLogAdd += ReadLogForMonologue;
    }

    private void OnDisable()
    {
        GameLog.Instance.OnLogAdd -= () => StartCoroutine(ReadLogDialogue());
        GameLog.Instance.OnLogAdd -= ReadLogForMonologue;
    }

    private void Update()
    {
        if (waintingHint)
        {
            if (hintWaitTimer > 0f) hintWaitTimer -= Time.deltaTime;
            else
            {
                ShowHint();
            }
        }

        if (reactionTimer > 0f) reactionTimer -= Time.deltaTime;
    }

    private IEnumerator ReadLogDialogue()
    {
        string lastLog = GameLog.Instance.gameLog[^1].log;

        var compatibleDialogues = dialogues.Where(x => x.eventCode == lastLog && !x.discovered);
        
        if(!compatibleDialogues.Any()) yield break;

        UniqueDialogueEvent dialogue = compatibleDialogues.First();
        
        DialogueManager.Instance.EndDialogue();

        yield return new WaitForSeconds(0.5f);

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

    private void ReadLogForMonologue()
    {
        if(reactionTimer > 0) return;
        
        var gameLog = GameLog.Instance.gameLog;

        var compatibleMonologues = monologues.Where(x =>
        {
            if (gameLog.Count < x.logPattern.Count) return false;

            for (int i = 0; i < x.logPattern.Count; i++)
            {
                string toCompare = x.logPattern[i];
                string pattern = gameLog[^(i + 1)].log;

                if (!ComparePatterns(pattern, toCompare)) return false;
            }

            return true;
        });
        
        if(!compatibleMonologues.Any()) return;

        MonologueReaction monologue = compatibleMonologues.First();

        reactionTimer = reactionCooldown + monologue.reactionTime;
        MonologueManager.Instance.StartMonologue(monologue.monologue);
    }

    private void ShowHint()
    {
        hintWaitTimer = lastDialogue.hintTime;
        DialogueManager.Instance.EndDialogue();
        DialogueManager.Instance.StartDialogue(lastDialogue.dialogue);
    }

    private bool ComparePatterns(string pattern, string toCompare)
    {
        List<string> patternSplitted = pattern.Split("/").ToList();
        List<string> toCompareSplitted = toCompare.Split("/").ToList();

        if (patternSplitted.Count < toCompareSplitted.Count) return false;
        
        for (int i = 0; i < toCompareSplitted.Count; i++)
        {
            if (patternSplitted[i] != toCompareSplitted[i]) return false;
        }

        return true;
    }
}
