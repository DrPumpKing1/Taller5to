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

    [Header("Other Settings Buttons")]
    [SerializeField] private Button backToLastCheckpointButton;
    [SerializeField] private Button dematerializeAllObjectsButton;
    [SerializeField] private Button skipDialogueButton;
    [SerializeField] private Button clearInstructionButton;
    [SerializeField] private Button skipBossPhaseButton;

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

        backToLastCheckpointButton.onClick.AddListener(BackToLastCheckpoint);
        dematerializeAllObjectsButton.onClick.AddListener(DematerializeAllObjects);
        skipDialogueButton.onClick.AddListener(SkipDialogue);
        clearInstructionButton.onClick.AddListener(ClearInstruction);
        skipBossPhaseButton.onClick.AddListener(SkipBossPhase);
    }
    private void RestartGame() => LevelSkipManager.Instance.SkipLevel(0);
    private void GoToLevel1() => LevelSkipManager.Instance.SkipLevel(1);
    private void GoToLevel2() => LevelSkipManager.Instance.SkipLevel(2);
    private void GoToLevel3() => LevelSkipManager.Instance.SkipLevel(3);
    private void GoToBoss() => LevelSkipManager.Instance.SkipLevel(4);

    private void BackToLastCheckpoint() => ScenesManager.Instance.FadeReloadCurrentScene();
    private void DematerializeAllObjects() => ProjectionManager.Instance.DematerializeAllObjects();
    private void SkipDialogue() => DialogueManager.Instance.EndDialogue();
    private void ClearInstruction() => InstructionsManager.Instance.HideInstruction();
    private void SkipBossPhase() => BossPhaseHandler.Instance.ForceChangeToNextPhase();
}
