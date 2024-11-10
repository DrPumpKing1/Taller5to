using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScriptGameplay : MonoBehaviour
{
    private void Update()
    {
        CheckSkipDialogue();
        CheckHideInstruction();
        CheckSkipBossPhase();
        //CheckSkipCinematic();
        //CheckReloadScene();

        /*
        CheckSkipLevel1();
        CheckSkipLevel2();
        CheckSkipLevel3();
        CheckSkipBoss();
        */
    }

    private void CheckHideInstruction()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            InstructionsManager.Instance.HideInstruction();
        }
    }
    private void CheckSkipDialogue()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DialogueManager.Instance.EndDialogue();
        }
    }

    private void CheckSkipBossPhase()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            BossPhaseHandler.Instance.ForceChangeToNextPhase();
        }
    }

    private void CheckReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ScenesManager.Instance.FadeReloadCurrentScene();
        }
    }

    private void CheckSkipCinematic()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CinematicsManager.Instance.SkipCinematic();
        }
    }

    private void CheckSkipLevel1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LevelSkipManager.Instance.SkipLevel(1);
        }
    }
    private void CheckSkipLevel2()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            LevelSkipManager.Instance.SkipLevel(2);
        }
    }

    private void CheckSkipLevel3()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            LevelSkipManager.Instance.SkipLevel(3);
        }
    }

    private void CheckSkipBoss()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            LevelSkipManager.Instance.SkipLevel(4);
        }
    }
}
