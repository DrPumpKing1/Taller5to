using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicFadeHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sceneFadeInTime;
    [SerializeField] private float sceneFadeOutTime;
    [SerializeField] private List<string> exceptionScenes;
    

    private void OnEnable()
    {
        ScenesManager.OnSceneTransitionOutStart += ScenesManager_OnSceneTransitionOutStart;
        ScenesManager.OnSceneTransitionInStart += ScenesManager_OnSceneTransitionInStart;
    }

    private void OnDisable()
    {
        ScenesManager.OnSceneTransitionOutStart -= ScenesManager_OnSceneTransitionOutStart;
        ScenesManager.OnSceneTransitionInStart -= ScenesManager_OnSceneTransitionInStart;
    }

    private void ScenesManager_OnSceneTransitionInStart(object sender, ScenesManager.OnSceneLoadEventArgs e)
    {
        if (IsExceptionScene(e.sceneName)) return;
        MusicFadeManager.Instance.FadeInMusic(sceneFadeInTime);
    }

    private void ScenesManager_OnSceneTransitionOutStart(object sender, ScenesManager.OnSceneLoadEventArgs e)
    {
        if (IsExceptionScene(e.sceneName)) return;
        MusicFadeManager.Instance.FadeOutMusic(sceneFadeOutTime);
    }

    private bool IsExceptionScene(string sceneName)
    {
        foreach(string exceptionScene in exceptionScenes)
        {
            if (exceptionScene == sceneName) return true;
        }

        return false;
    }
}
