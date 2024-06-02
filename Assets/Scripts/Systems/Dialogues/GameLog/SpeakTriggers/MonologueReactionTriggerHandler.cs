using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonologueReactionTriggerHandler : MonoBehaviour
{
    [Serializable]
    public class MonologueReaction
    {
        public MonologueSO monologue;
        public float reactionTime;
        public List<string> logPattern;
    }

    [Header("Monologues")]
    [SerializeField] private float reactionCooldown;
    [SerializeField] private float reactionTimer;

    [SerializeField] private List<MonologueReaction> monologues;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += ReadLogForMonologue;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= ReadLogForMonologue;
    }

    private void Update()
    {
        HandleReactionTimer();
    }

    private void HandleReactionTimer()
    {
        if (reactionTimer > 0f) reactionTimer -= Time.deltaTime;
    }

    private void ReadLogForMonologue()
    {
        if (reactionTimer > 0) return;

        var gameLog = GameLogManager.Instance.GameLog;

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

        if (!compatibleMonologues.Any()) return;

        MonologueReaction monologue = compatibleMonologues.First();

        reactionTimer = reactionCooldown + monologue.reactionTime;
        MonologueManager.Instance.StartMonologue(monologue.monologue);
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
