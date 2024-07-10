using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicFadeHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float sceneFadeInTime;
    [SerializeField] private float sceneFadeOutTime;
    [SerializeField] private List<ExceptionTransition> exceptionTransitions;
    
    [System.Serializable]
    public class ExceptionTransition
    {
        public string originScene;
        public string targetScene;
    }

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
        if (IsExceptionTransition(e.originScene, e.targetScene)) return;
        MusicFadeManager.Instance.FadeInMusic(sceneFadeInTime);
    }

    private void ScenesManager_OnSceneTransitionOutStart(object sender, ScenesManager.OnSceneLoadEventArgs e)
    {
        if (IsExceptionTransition(e.originScene, e.targetScene)) return;
        MusicFadeManager.Instance.FadeOutMusic(sceneFadeOutTime);
    }

    private bool IsExceptionTransition(string originScene, string targetScene)
    {
        foreach(ExceptionTransition exceptionTransition in exceptionTransitions)
        {
            if (exceptionTransition.targetScene == targetScene && exceptionTransition.originScene == originScene) return true;
        }

        return false;
    }
}
