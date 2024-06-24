using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUIGraphicsContentsButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private OptionsUIContentsHandler optionsUIContentsHandler;

    [Header("Back To Main Content Button")]
    [SerializeField] private Button backToMainContentButton;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        backToMainContentButton.onClick.AddListener(ShowMainContent);
    }

    private void ShowMainContent() => optionsUIContentsHandler.ShowMainContent();
}
