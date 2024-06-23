using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalSFXController : MonoBehaviour
{
    private void OnEnable()
    {
        ScenesManager.OnSceneLoadStart += ScenesManager_OnSceneLoadStart;
    }
    private void OnDisable()
    {
        ScenesManager.OnSceneLoadStart -= ScenesManager_OnSceneLoadStart;
    }

    private void ScenesManager_OnSceneLoadStart(object sender, ScenesManager.OnSceneLoadEventArgs e)
    {
        Destroy(gameObject);
    }
}
