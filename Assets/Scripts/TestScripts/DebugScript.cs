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
        CheckHideInstruction();

        CheckSkipLevel1();
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
            ScenesManager.Instance.FadeReloadCurrentScene();
        }
    }

    private void CheckHideInstruction()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            InstructionsManager.Instance.HideInstruction();
        }
    }

    private void CheckSkipLevel1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LevelSkipManager.Instance.SkipLevel(1);
        }
    }
}
