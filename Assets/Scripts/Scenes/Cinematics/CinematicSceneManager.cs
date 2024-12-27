using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CinematicSceneManager : MonoBehaviour
{
    public static CinematicSceneManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Settings")]
    [SerializeField] private string nextScene;

    private const float SCENE_FADE_OUT_TIME = 0.5f;

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        StartCoroutine(CinematicCoroutine());
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one CinematicSceneManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private IEnumerator CinematicCoroutine()
    {
        float duration = videoPlayer.frameCount / (float)videoPlayer.frameRate;
        float ininterruptedDuration = duration - SCENE_FADE_OUT_TIME;

        yield return new WaitForSeconds(ininterruptedDuration);

        ScenesManager.Instance.FadeLoadTargetScene(nextScene);
    }

    public void SkipCinematic()
    {
        ScenesManager.Instance.FadeLoadTargetScene(nextScene);
    }
}
