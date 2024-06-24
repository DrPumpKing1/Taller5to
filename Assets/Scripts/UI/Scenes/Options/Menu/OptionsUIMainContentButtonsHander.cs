using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OptionsUIMainContentButtonsHander : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private OptionsUIContentsHandler optionsContentsHandler;

    [Header("Audio Button")]
    [SerializeField] private Button audioButton;

    [Header("Graphics Button")]
    [SerializeField] private Button graphicsButton;


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
        graphicsButton.onClick.AddListener(ShowGraphicsContent);
        backToMenuButton.onClick.AddListener(BackToMenu);
    }

    private void ShowAudioContent() => optionsContentsHandler.ShowAudioContent();
    private void ShowGraphicsContent() => optionsContentsHandler.ShowGraphicsContent();

    private void BackToMenu()
    {
        ScenesManager.Instance.FadeLoadTargetScene(menuScene);
    }
}
