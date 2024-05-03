using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseUIButtonsHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Button inGameOptionsButton;

    public static event EventHandler OnOpenInGameOptionsUI;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        inGameOptionsButton.onClick.AddListener(OpenInGameOptionsUI);
    }

    private void OpenInGameOptionsUI()
    {
        OnOpenInGameOptionsUI?.Invoke(this, EventArgs.Empty);
    }

}
