using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainMenuUIButtonsHandler : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button deleteDataButton;

    [Header("Scenes")]
    [SerializeField] private string startCinematicScene;
    [SerializeField] private string gameplayScene;
    [SerializeField] private string optionsScene;
    [SerializeField] private string creditsScene;

    [Header("Data Paths")]
    [SerializeField] private DataPathsSO dataPathsSO;

    private void Awake()
    {
        InitializeButtonsListeners();
    }
    private void Start()
    {
        CheckContiueButtonAvailable();
    }


    private void InitializeButtonsListeners()
    {
        newGameButton.onClick.AddListener(StartNewGame);
        continueButton.onClick.AddListener(LoadGameScene);
        optionsButton.onClick.AddListener(LoadOptionsScene);
        creditsButton.onClick.AddListener(LoadCreditsScene);
        quitButton.onClick.AddListener(QuitGame);
        deleteDataButton.onClick.AddListener(DeleteData);
    }

    private void CheckContiueButtonAvailable()
    {
        if (!GeneralDataMethods.CheckIfDataPathsExist(dataPathsSO.dataPaths)) SetContinueButton(false);
        else SetContinueButton(true);
    }

    private void SetContinueButton(bool enable)
    {
        if(enable) continueButton.gameObject.SetActive(true);
        else continueButton.gameObject.SetActive(false);
    }

    private void StartNewGame()
    {
        GeneralDataMethods.DeleteDataInPaths(dataPathsSO.dataPaths);
        LoadStartCinematicScene();
    }

    private void LoadStartCinematicScene()=> ScenesManager.Instance.FadeLoadTargetScene(startCinematicScene);
    private void LoadGameScene()=> ScenesManager.Instance.FadeLoadTargetScene(gameplayScene);
    private void LoadOptionsScene() => ScenesManager.Instance.FadeLoadTargetScene(optionsScene);
    private void LoadCreditsScene() => ScenesManager.Instance.FadeLoadTargetScene(creditsScene);
    private void QuitGame() => ScenesManager.Instance.QuitGame();
    private void DeleteData()
    {
        GeneralDataMethods.DeleteDataInPaths(dataPathsSO.dataPaths);
        CheckContiueButtonAvailable();
    }     
}
