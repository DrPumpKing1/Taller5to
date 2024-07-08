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

    public List<string> storedLogPattern = new List<string>();

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Update()
    {
        HandleReactionTimer();
    }

    private void HandleReactionTimer()
    {
        if (reactionTimer > 0f) reactionTimer -= Time.deltaTime;
    }

    private void HandleReactionMonologue(string log)
    {
        if (reactionTimer > 0) return;

        storedLogPattern.Add(log);

        var compatibleMonologues = monologues.Where(x =>
        {
            if (storedLogPattern.Count < x.logPattern.Count) return false;

            for (int i = 0; i < x.logPattern.Count; i++)
            {
                string toCompare = x.logPattern[i];
                string pattern = storedLogPattern[^(i + 1)];

                if (!ComparePatterns(pattern, toCompare)) return false;
            }

            return true;
        });

        if (!compatibleMonologues.Any()) return;

        MonologueReaction monologue = compatibleMonologues.First();

        reactionTimer = reactionCooldown + monologue.reactionTime;

        MonologueManager.Instance.StartMonologue(monologue.monologue);
        storedLogPattern.Clear();
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

    #region GameLog Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        HandleReactionMonologue(e.gameplayAction.log);
    }
    #endregion
}
