using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProjectableObjectUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private Image projectableObjectImage;
    [SerializeField] private Image keybindImage;
    [SerializeField] private TextMeshProUGUI projectableObjectName;

    [Header("Settings")]
    [SerializeField] private int index;

    public int Index => index;

    private const string SELECTION_TRIGGER = "Select";
    private const string DESELECTION_TRIGGER = "Deselect";

    public void SetUI(ProjectableObjectSO projectableObjectSO)
    {
        projectableObjectImage.sprite = projectableObjectSO.sprite;
        keybindImage.sprite = projectableObjectSO.keyBindSprite;
        projectableObjectName.text = projectableObjectSO.objectName;
    }

    public void SelectUI()
    {
        animator.ResetTrigger(DESELECTION_TRIGGER);
        animator.SetTrigger(SELECTION_TRIGGER);
    }

    public void DeselectUI()
    {
        animator.ResetTrigger(SELECTION_TRIGGER);
        animator.SetTrigger(DESELECTION_TRIGGER);
    }
}
