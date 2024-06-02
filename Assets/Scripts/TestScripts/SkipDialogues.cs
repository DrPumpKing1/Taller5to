using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipDialogues : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DialogueManager.Instance.EndDialogue();
        }
    }
}
