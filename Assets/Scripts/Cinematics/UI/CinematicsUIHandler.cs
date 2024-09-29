using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

public class CinematicsUIHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CinematicsTransitionPanelUIHandler cinematicsTransitionPanelUIHandler;
    [SerializeField] private CinematicsVideoUIHandler cinematicsVideoUIHandler;

    public static event EventHandler OnCinematicUIStarting;
    public static event EventHandler OnCinematicUIStart;
    public static event EventHandler OnCinematicUIEnding;
    public static event EventHandler OnCinematicUIEnd;

    private void OnEnable()
    {
        CinematicsManager.OnCinematicStart += CinematicsManager_OnCinematicStart;
        CinematicsManager.OnCinematicEndDirect += CinematicsManager_OnCinematicEndDirect;
    }

    private void OnDisable()
    {
        CinematicsManager.OnCinematicStart -= CinematicsManager_OnCinematicStart;
        CinematicsManager.OnCinematicEndDirect -= CinematicsManager_OnCinematicEndDirect;
    }

    private IEnumerator PlayCinematic(VideoClip videoClip)
    {
        float duration = videoClip.frameCount / (float)videoClip.frameRate;
        float ininterruptedDuration = duration - cinematicsTransitionPanelUIHandler.FullBlackTime;

        OnCinematicUIStarting?.Invoke(this, EventArgs.Empty);

        cinematicsTransitionPanelUIHandler.ShowTransitionPanel();
        yield return new WaitForSeconds(cinematicsTransitionPanelUIHandler.TransitionTime);
        yield return new WaitForSeconds(cinematicsTransitionPanelUIHandler.FullBlackTime);
        cinematicsTransitionPanelUIHandler.HideTransitionPanel();

        OnCinematicUIStart?.Invoke(this, EventArgs.Empty);

        cinematicsVideoUIHandler.PlayVideoComplete(videoClip);

        yield return new WaitForSeconds(ininterruptedDuration);

        OnCinematicUIEnding?.Invoke(this, EventArgs.Empty);

        cinematicsTransitionPanelUIHandler.ShowTransitionPanel();
        yield return new WaitForSeconds(cinematicsTransitionPanelUIHandler.TransitionTime);
        yield return new WaitForSeconds(cinematicsTransitionPanelUIHandler.FullBlackTime);

        cinematicsVideoUIHandler.StopVideoComplete();
        cinematicsTransitionPanelUIHandler.HideTransitionPanel();

        OnCinematicUIEnd?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator EndCinematic()
    {
        OnCinematicUIEnding?.Invoke(this, EventArgs.Empty);

        cinematicsTransitionPanelUIHandler.ShowTransitionPanel();
        yield return new WaitForSeconds(cinematicsTransitionPanelUIHandler.TransitionTime);
        yield return new WaitForSeconds(cinematicsTransitionPanelUIHandler.FullBlackTime);

        cinematicsVideoUIHandler.StopVideoComplete();
        cinematicsTransitionPanelUIHandler.HideTransitionPanel();

        OnCinematicUIEnd?.Invoke(this, EventArgs.Empty);
    }

    #region CinematicsManager Subsctiptions
    private void CinematicsManager_OnCinematicStart(object sender, CinematicsManager.OnCinematicEventArgs e)
    {
        StartCoroutine(PlayCinematic(e.cinematic.videoClip));
    }

    private void CinematicsManager_OnCinematicEndDirect(object sender, CinematicsManager.OnCinematicEventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(EndCinematic());
    }
    #endregion
}
