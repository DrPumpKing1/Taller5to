using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueTriggerHandler : MonoBehaviour
{
    [Serializable]
    public class DialogueEvent
    {
        public int id;
        public DialogueSO dialogue;
        public string eventCode;
    }

    [Header("Enabler")]
    [SerializeField] private bool enableTriggerDialogues;

    [Header("Dialogues")]
    [SerializeField] private DialogueEvent lastDialogueEvent;
    [Space]
    [SerializeField] private float dialogueReplacementSafeTime = 1f;

    [Header("Collections")]
    [SerializeField] private List<DialogueEvent> dialogues;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private IEnumerator ReadLogDialogueCoroutine(string log)
    {
        //if (MonologueManager.Instance.PlayingMonologue()) yield break; //Dialogues can't play while another monologue is playing
        //if (DialogueManager.Instance.PlayingDialogue()) yield break; //Dialogues can't play while another dialogue is playing

        var compatibleDialogues = dialogues.Where(x => x.eventCode == log);

        if (!compatibleDialogues.Any()) yield break;

        DialogueEvent dialogueEvent = compatibleDialogues.First();

        if (dialogueEvent != null)
        {
            if (dialogueEvent.dialogue == DialogueManager.Instance.CurrentDialogueSO) yield break;
        }

        if (DialogueManager.Instance.PlayingDialogue())
        {
            DialogueManager.Instance.EndDialogue();
            yield return new WaitForSeconds(dialogueReplacementSafeTime);
        }

        if (MonologueManager.Instance.PlayingMonologue())
        {
            MonologueManager.Instance.EndMonologue();
            yield return new WaitForSeconds(dialogueReplacementSafeTime);
        }

        DialogueManager.Instance.StartDialogue(dialogueEvent.dialogue);

        lastDialogueEvent = dialogueEvent;
    }

    #region GameLogSubscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (!enableTriggerDialogues) return;

        StartCoroutine(ReadLogDialogueCoroutine(e.gameplayAction.log));
    }
    #endregion
}
