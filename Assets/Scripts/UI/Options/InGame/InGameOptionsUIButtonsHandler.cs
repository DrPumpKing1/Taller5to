using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameOptionsUIButtonsHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button inGameAudioOptionsButton;

    public static event EventHandler OnOpenInGameAudioOptionsUI;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        inGameAudioOptionsButton.onClick.AddListener(OpenInGameAudioOptionsUI);
    }

    private void OpenInGameAudioOptionsUI()
    {
        OnOpenInGameAudioOptionsUI?.Invoke(this, EventArgs.Empty);
    }

}
