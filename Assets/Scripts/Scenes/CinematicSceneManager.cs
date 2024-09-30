using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CinematicSceneManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Settings")]
    [SerializeField] private string nextScene;

    private const float sceneFadeOutTime = 0.5f;

    private void Start()
    {
        StartCoroutine(CinematicCoroutine());
    }

    private IEnumerator CinematicCoroutine()
    {
        float duration = videoPlayer.frameCount / (float)videoPlayer.frameRate;
        float ininterruptedDuration = duration - sceneFadeOutTime;

        yield return new WaitForSeconds(ininterruptedDuration);

        ScenesManager.Instance.FadeLoadTargetScene(nextScene);
    }
}
