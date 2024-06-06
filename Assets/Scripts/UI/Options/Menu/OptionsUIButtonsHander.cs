using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OptionsUIButtonsHander : MonoBehaviour
{
    [Header("AudioButton Button")]
    [SerializeField] private Button audioButton;

    [Header("Back To Menu Button")]
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private string menuScene;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        audioButton.onClick.AddListener(ShowAudioPanel);
        backToMenuButton.onClick.AddListener(BackToMenu);
    }

    private void ShowAudioPanel()
    {
        //
    }

    private void BackToMenu()
    {
        ScenesManager.Instance.SimpleLoadTargetScene(menuScene);
    }
}
