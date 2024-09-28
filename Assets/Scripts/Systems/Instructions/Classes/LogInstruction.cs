using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInstruction : Instruction
{
    [Header("Log To Show")]
    [SerializeField] protected string logToShow;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToShow) return;
        CheckShouldShow();
    }
}
