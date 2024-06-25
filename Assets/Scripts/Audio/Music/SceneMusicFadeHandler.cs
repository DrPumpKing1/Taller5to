using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicFadeHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sceneFadeInTime;
    [SerializeField] private float sceneFadeOutTime;

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
        MusicFadeManager.Instance.FadeInMusic(sceneFadeInTime);
    }

    private void ScenesManager_OnSceneTransitionOutStart(object sender, ScenesManager.OnSceneLoadEventArgs e)
    {
        MusicFadeManager.Instance.FadeOutMusic(sceneFadeOutTime);
    }
}
