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
    [SerializeField] private Button bossFightButton;

    [Header("Other Settings Buttons")]
    [SerializeField] private Button backToLastCheckpointButton;
    [SerializeField] private Button dematerializeAllObjectsButton;
    [SerializeField] private Button skipDialogueButton;
    [SerializeField] private Button clearInstructionButton;
    [SerializeField] private Button skipShowcasePhaseButton;
    [SerializeField] private Button skipBossPhaseButton;

    [Header("ExtraSettings Settings")]
    [SerializeField] private Toggle directionalLightToggle;
    [SerializeField] private Button simulateFallButton;

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
        bossFightButton.onClick.AddListener(GoToBossFight);

        backToLastCheckpointButton.onClick.AddListener(BackToLastCheckpoint);
        dematerializeAllObjectsButton.onClick.AddListener(DematerializeAllObjects);
        skipDialogueButton.onClick.AddListener(SkipDialogue);
        clearInstructionButton.onClick.AddListener(ClearInstruction);
        skipShowcasePhaseButton.onClick.AddListener(SkipShowcasePhase);
        skipBossPhaseButton.onClick.AddListener(SkipBossPhase);

        directionalLightToggle.onValueChanged.AddListener(ToggleOverrideDirectionalLight);
        simulateFallButton.onClick.AddListener(SimulateFall);
    }

    private void RestartGame() => LevelSkipManager.Instance.SkipLevel(0);
    private void GoToLevel1() => LevelSkipManager.Instance.SkipLevel(1);
    private void GoToLevel2() => LevelSkipManager.Instance.SkipLevel(2);
    private void GoToLevel3() => LevelSkipManager.Instance.SkipLevel(3);
    private void GoToBoss() => LevelSkipManager.Instance.SkipLevel(4);
    private void GoToBossFight() => LevelSkipManager.Instance.SkipLevel(5);

    private void BackToLastCheckpoint() => ScenesManager.Instance.FadeReloadCurrentScene();
    private void DematerializeAllObjects() => ProjectionManager.Instance.DematerializeAllObjects();
    private void SkipDialogue() => DialogueManager.Instance.EndDialogue();
    private void ClearInstruction() => InstructionsManager.Instance.HideInstruction();
    private void SkipShowcasePhase() => ShowcaseRoomPhaseHandler.Instance.ForceChangeToNextPhase();
    private void SkipBossPhase() => BossPhaseHandler.Instance.ForceChangeToNextPhase();

    private void ToggleOverrideDirectionalLight(bool overrideDirectionalLight) => DirectionalLightingHandler.Instance.OverrideOnDirectionalLights(overrideDirectionalLight);
    private void SimulateFall() => PlayerLand.Instance.SimulateSoftLanding();
}
