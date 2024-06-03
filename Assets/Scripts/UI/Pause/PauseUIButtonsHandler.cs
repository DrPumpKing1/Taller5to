using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseUIButtonsHandler : MonoBehaviour
{
    [Header("InGameOptions Button")]
    [SerializeField] private Button inGameOptionsButton;

    [Header("BackToMenu Button")]
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private string mainMenuScene;

    public static event EventHandler OnOpenInGameOptionsUI;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        inGameOptionsButton.onClick.AddListener(OpenInGameOptionsUI);
        backToMenuButton.onClick.AddListener(BackToMenu);
    }

    private void OpenInGameOptionsUI()
    {
        OnOpenInGameOptionsUI?.Invoke(this, EventArgs.Empty);
    }

    private void BackToMenu()
    {
        ScenesManager.Instance.SimpleLoadTargetScene(mainMenuScene);
    }
}
