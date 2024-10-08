using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevMenuButtonsHandler : MonoBehaviour
{
    [Header("Stage Select Buttons")]
    [SerializeField] private Button restartGameButton;
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;
    [SerializeField] private Button bossButton;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        restartGameButton.onClick.AddListener(RestartGame);
        level1Button.onClick.AddListener(GoToLevel1);
        level2Button.onClick.AddListener(GoToLevel2);
        level3Button.onClick.AddListener(GoToLevel3);
        bossButton.onClick.AddListener(GoToBoss);
    }
    private void RestartGame() => LevelSkipManager.Instance.SkipLevel(0);
    private void GoToLevel1() => LevelSkipManager.Instance.SkipLevel(1);
    private void GoToLevel2() => LevelSkipManager.Instance.SkipLevel(2);
    private void GoToLevel3() => LevelSkipManager.Instance.SkipLevel(3);
    private void GoToBoss() => LevelSkipManager.Instance.SkipLevel(4);
}
