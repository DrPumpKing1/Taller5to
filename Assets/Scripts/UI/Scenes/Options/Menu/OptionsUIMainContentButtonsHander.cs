using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OptionsUIMainContentButtonsHander : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private OptionsUIContentsHandler optionsContentsHandler;

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
        audioButton.onClick.AddListener(ShowAudioContent);
        backToMenuButton.onClick.AddListener(BackToMenu);
    }

    private void ShowAudioContent() => optionsContentsHandler.ShowAudioContent();

    private void BackToMenu()
    {
        ScenesManager.Instance.FadeLoadTargetScene(menuScene);
    }
}
