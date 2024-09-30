using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;


public class MainMenuUIButtonsHandler : MonoBehaviour
{
    [Header("Play Button")]
    [SerializeField] private Button playButton;
    [SerializeField] private string startCinematicScene;
    [SerializeField] private string gameplayScene;
    [Space]
    [SerializeField] private string dataPathToCheck;

    [Header("Options Button")]
    [SerializeField] private Button optionsButton;
    [SerializeField] private string optionsScene;

    [Header("Credits Button")]
    [SerializeField] private Button creditsButton;
    [SerializeField] private string creditsScene;

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
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayGame()
    {
        if (CheckIfDataPathExists()) StartGame();
        else PlayFirstCinematic();
    }

    private bool CheckIfDataPathExists()
    {
        string dirPath = Application.persistentDataPath;
        string path = Path.Combine(dirPath, dataPathToCheck);

        if (File.Exists(path)) return true;

        return false;
    }

    private void PlayFirstCinematic()
    {
        ScenesManager.Instance.FadeLoadTargetScene(startCinematicScene);
    }

    private void StartGame()
    {
        ScenesManager.Instance.FadeLoadTargetScene(gameplayScene);
    }

    private void Options()
    {
        ScenesManager.Instance.FadeLoadTargetScene(optionsScene);
    }

    private void Credits()
    {
        ScenesManager.Instance.FadeLoadTargetScene(creditsScene);
    }

    private void QuitGame() => ScenesManager.Instance.QuitGame();
}
