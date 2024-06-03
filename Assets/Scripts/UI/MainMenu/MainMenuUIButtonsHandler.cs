using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class MainMenuUIButtonsHandler : MonoBehaviour
{
    [Header("Play Button")]
    [SerializeField] private Button playButton;
    [SerializeField] private string gameplayScene;

    [Header("Options Button")]
    [SerializeField] private Button optionsButton;
    [SerializeField] private string optionsScene;

    [Header("Quit Button")]
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(Options);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        ScenesManager.Instance.SimpleLoadTargetScene(gameplayScene);
    }

    private void Options()
    {
        //ScenesManager.Instance.SimpleLoadTargetScene(optionsScene);
    }

    private void QuitGame() => ScenesManager.Instance.QuitGame();
}
