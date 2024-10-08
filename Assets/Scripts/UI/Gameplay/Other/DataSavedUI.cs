using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSavedUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField, Range(0, 1f)] private float timeToShow;
    [SerializeField, Range(2f, 5f)] private float timeShowing;
    [SerializeField, Range(0.5f, 2f)] private float transitionTime;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const string HIDDEN_ANIMATION_NAME = "DataSavedHidden";

    private void OnEnable()
    {
        CheckpointManager.OnCheckpointReached += CheckpointManager_OnCheckpointReached;
    }

    private void OnDisable()
    {
        CheckpointManager.OnCheckpointReached -= CheckpointManager_OnCheckpointReached;
    }

    private void ShowDataSavedUI()
    {
        animator.ResetTrigger(HIDE_TRIGGER);
        animator.SetTrigger(SHOW_TRIGGER);
    }

    private void HideDataSavedUI()
    {
        animator.ResetTrigger(SHOW_TRIGGER);
        animator.SetTrigger(HIDE_TRIGGER);
    }

    private void ShowDataSavedText()
    {
        StopAllCoroutines();
        StartCoroutine(ShowDataSavedTextCoroutine());
    }

    private IEnumerator ShowDataSavedTextCoroutine()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(HIDDEN_ANIMATION_NAME) || animator.IsInTransition(0))
        {
            yield return HideDataSavedTextCoroutine();
        }

        yield return new WaitForSeconds(timeToShow);

        ShowDataSavedUI();

        yield return new WaitForSeconds(timeShowing);

        HideDataSavedUI();
    }

    private IEnumerator HideDataSavedTextCoroutine()
    {
        HideDataSavedUI();
        yield return new WaitForSeconds(transitionTime);
    }

    #region CheckpointManagerSubscriptions
    private void CheckpointManager_OnCheckpointReached(object sender, CheckpointManager.OnCheckpointReachedEventArgs e)
    {
        ShowDataSavedText();
    }
    #endregion
}
