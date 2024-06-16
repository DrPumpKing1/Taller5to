using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class Instruction : MonoBehaviour
{
    [Header("Log")]
    [SerializeField] protected string logToAcomplish;

    protected bool hasBeenAcomplished;
    protected bool isShowing;

    public static event EventHandler<OnInstructionEventArgs> OnInstructionShow;
    public static event EventHandler<OnInstructionEventArgs> OnInstructionHide;

    public class OnInstructionEventArgs
    {
        public Instruction instruction;
    }

    protected virtual void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }
    protected virtual void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckShouldHide();
    }

    private void InitializeVariables()
    {
        hasBeenAcomplished = false;
        isShowing = false;
    }

    protected void ShowInstruction()
    {
        OnInstructionShow?.Invoke(this, new OnInstructionEventArgs { instruction = this });
        isShowing = true;
    }
    protected void HideInstruction()
    {
        OnInstructionHide?.Invoke(this, new OnInstructionEventArgs { instruction = this });
        isShowing = false;
    }

    protected void CheckShouldHide()
    {
        if (!isShowing) return;
        if (!hasBeenAcomplished) return;

        HideInstruction();
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
}
