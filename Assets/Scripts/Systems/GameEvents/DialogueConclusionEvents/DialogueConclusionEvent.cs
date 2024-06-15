using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueConclusionEvent : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueSO dialogueSO;

    private void OnEnable()
    {
        DialogueManager.OnDialogueEnd += DialogueManager_OnDialogueEnd;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueEnd -= DialogueManager_OnDialogueEnd;
    }

    private void DialogueManager_OnDialogueEnd(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        if (e.dialogueSO != dialogueSO) return;

        TriggerEvent();
    }

    protected abstract void TriggerEvent();
}
