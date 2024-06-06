using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OptionsUIAudioContentButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private OptionsContentsHandler optionsContentsHandler;

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

    private void ShowMainContent() => optionsContentsHandler.ShowMainContent();
}
