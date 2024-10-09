using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerDialogues : MonoBehaviour
{
    private void OnEnable()
    {
        DialogueManager.OnDialogueStart += DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd += DialogueManager_OnDialogueEnd;

        MonologueManager.OnMonologueStart += MonologueManager_OnMonologueStart;
        MonologueManager.OnMonologueEnd += MonologueManager_OnMonologueEnd;
    }
    private void OnDisable()
    {
        DialogueManager.OnDialogueStart -= DialogueManager_OnDialogueStart;
        DialogueManager.OnDialogueEnd -= DialogueManager_OnDialogueEnd;

        MonologueManager.OnMonologueStart -= MonologueManager_OnMonologueStart;
        MonologueManager.OnMonologueEnd -= MonologueManager_OnMonologueEnd;
    }

    private void DialogueManager_OnDialogueStart(object sender, DialogueManager.OnDialogueEventArgs e)
    {
        GameLogManager.Instance.Log($"Dialogues/Start/{e.dialogueSO.id}");
        GameLogManager.Instance.Log($"Dialogues/Any/Start");
    }
        
    private void DialogueManager_OnDialogueEnd(object sender, DialogueManager.OnDialogueEventArgs e) 
    {
        GameLogManager.Instance.Log($"Dialogues/End/{e.dialogueSO.id}");
        GameLogManager.Instance.Log($"Dialogues/Any/End");
    }

    private void MonologueManager_OnMonologueStart(object sender, MonologueManager.OnMonologueEventArgs e)
    {
       GameLogManager.Instance.Log($"Monologues/Start/{e.monologueSO.id}");
       GameLogManager.Instance.Log($"Monologues/Any/Start");
    }

    private void MonologueManager_OnMonologueEnd(object sender, MonologueManager.OnMonologueEventArgs e)
    {
        GameLogManager.Instance.Log($"Monologues/End/{e.monologueSO.id}");
        GameLogManager.Instance.Log($"Monologues/Any/End");
    }
}
