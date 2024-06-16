using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWASDToMoveInstruction : LogAcomplishedInstruction
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameStartEnd.OnGameStartEnd += GameStartEnd_OnGameStartEnd;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameStartEnd.OnGameStartEnd -= GameStartEnd_OnGameStartEnd;
    }

    private void GameStartEnd_OnGameStartEnd(object sender, System.EventArgs e)
    {
        CheckShouldShow();
    }
}
