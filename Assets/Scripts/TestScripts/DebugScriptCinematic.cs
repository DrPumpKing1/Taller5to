using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScriptCinematic : MonoBehaviour
{
    private void Update()
    {
        //CheckSkipCinematic();
    }

    private void CheckSkipCinematic()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CinematicSceneManager.Instance.SkipCinematic();
        }
    }
}
