using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConclusionInstruction : Instruction
{
    [Header("Dialogue Concluded To Show")]
    [SerializeField] protected DialogueSO dialogueSO;

    protected override void OnEnable()
    {
        base.OnEnable();
        DialogueManager.OnDialogueEnd += DialogueManager_OnDialogueEnd;

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DialogueManager.OnDialogueEnd -= DialogueManager_OnDialogueEnd;
    }

    private void DialogueManager_OnDialogueEnd(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        if (e.dialogueSO != dialogueSO) return;
        CheckShouldShow();
    }
}
