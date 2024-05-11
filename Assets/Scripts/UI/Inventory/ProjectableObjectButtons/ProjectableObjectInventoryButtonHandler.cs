using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ProjectableObjectInventoryButtonHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ProjectableObjectSO projectableObjectSO;

    [Header("UI Components")]
    [SerializeField] private Button projectableObjectButton;

    public static event EventHandler<OnProjectableObjectButtonUIClickedEventArgs> OnProjectableObjectInventoryButtonClicked;

    public class OnProjectableObjectButtonUIClickedEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    private void OnEnable()
    {
        ProjectableObjectsLearningManager.OnProjectableObjectLearned += ProjectableObjectsLearningManager_OnProjectableObjectLearned;
    }

    private void OnDisable()
    {
        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectsLearningManager_OnProjectableObjectLearned;
    }

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        CheckButtonState();
    }

    private void InitializeButtonsListeners()
    {
        projectableObjectButton.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        OnProjectableObjectInventoryButtonClicked?.Invoke(this, new OnProjectableObjectButtonUIClickedEventArgs { projectableObjectSO = projectableObjectSO });
    }

    private void CheckButtonState()
    {
        if (ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned.Contains(projectableObjectSO))
        {
            ShowButton();
        }
        else
        {
            HideButton();
        }
    }

    private void ShowButton() => projectableObjectButton.gameObject.SetActive(true);
    private void HideButton() => projectableObjectButton.gameObject.SetActive(false);

    #region ProjectableObjectsLearningManager Subscriptions
    private void ProjectableObjectsLearningManager_OnProjectableObjectLearned(object sender, ProjectableObjectsLearningManager.OnProjectableObjectLearnedEventArgs e)
    {
        CheckButtonState();
    }
    #endregion
}
