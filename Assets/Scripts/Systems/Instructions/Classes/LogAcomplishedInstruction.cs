using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class LogAcomplishedInstruction : Instruction
{
    [Header("Log")]
    [SerializeField] protected string logToAcomplish;
    protected virtual void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }
    protected virtual void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    protected bool LogContainsLogToAcomplish()
    {
        foreach (GameLogManager.GameplayAction gameplayAction in GameLogManager.Instance.GameLog)
        {
            if (ComparePatterns(gameplayAction.log, logToAcomplish))
            {
                return true;
            }
        }

        return false;
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

    private void GameLogManager_OnLogAdd()
    {
        if (hasBeenAcomplished) return;
        if (!LogContainsLogToAcomplish()) return;

        hasBeenAcomplished = true;
    }

    protected override bool CheckCondition() => true;
}
