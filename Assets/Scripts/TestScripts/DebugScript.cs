using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScript : MonoBehaviour
{
    private void Update()
    {
        CheckSkipDialogue();
        CheckReloadScene();
    }

    private void CheckSkipDialogue()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DialogueManager.Instance.EndDialogue();
        }
    }

    private void CheckReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScenesManager.Instance.SimpleReloadCurrentScene();
        }
    }
}
