using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionGemUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private int gemNumber;

    public int GemNumber => gemNumber;

    private const string USE_TRIGGER = "Use";
    private const string RECOVER_TRIGGER = "Recover";

    public void HideProjectionGem()
    {
        animator.ResetTrigger(RECOVER_TRIGGER);
        animator.SetTrigger(USE_TRIGGER);
    }

    public void ShowProjectionGem()
    {
        animator.ResetTrigger(USE_TRIGGER);
        animator.SetTrigger(RECOVER_TRIGGER);
    }
}
