using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class Instruction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected string logToAcomplish;
    [SerializeField,Range(0f,3f)] protected float timeToShow;

    [Header("Booleans")]
    [SerializeField] protected bool hasBeenAcomplished;
    [SerializeField] protected bool isShowing;

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

    protected virtual void Update()
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

    protected void CheckShouldShow()
    {
        if (!CheckCondition()) return;
        if (hasBeenAcomplished) return;
        if (isShowing) return;

        StartCoroutine(ShowInstructionCoroutine());
    }

    private IEnumerator ShowInstructionCoroutine()
    {
        yield return new WaitForSeconds(timeToShow);
        ShowInstruction();
    }

    protected void CheckShouldHide()
    {
        if (!isShowing) return;
        if (!hasBeenAcomplished) return;

        StopAllCoroutines();
        HideInstruction();
    }

    protected virtual bool CheckCondition() => true;

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


    #region GameLog Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (!LogContainsLogToAcomplish()) return;
        if (hasBeenAcomplished) return;

        hasBeenAcomplished = true;
    }
    #endregion
}
