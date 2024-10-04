using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject parent;
    [SerializeField] private Animator feedbackAnimator;

    [Header("Settings")]
    [SerializeField] private float transitionTime;
    [SerializeField] private float showingTime;

    private const string SHOW_TRIGGER = "Show";
    private const string HIDE_TRIGGER = "Hide";

    private const float TRANSITION_TIME = 0.5f;

    public float TotalLifetime => showingTime + 2*transitionTime;

    protected virtual void Start()
    {
        StartCoroutine(ShowCoroutine());
    }

    private IEnumerator ShowCoroutine()
    {
        Show();

        yield return new WaitForSeconds(transitionTime);
        yield return new WaitForSeconds(showingTime);

        Hide();

        yield return new WaitForSeconds(transitionTime);

        Destroy(parent);
    }

    private void Show()
    {
        feedbackAnimator.ResetTrigger(HIDE_TRIGGER);
        feedbackAnimator.SetTrigger(SHOW_TRIGGER);
    }
    private void Hide()
    {
        feedbackAnimator.ResetTrigger(SHOW_TRIGGER);
        feedbackAnimator.SetTrigger(HIDE_TRIGGER);
    }
}
