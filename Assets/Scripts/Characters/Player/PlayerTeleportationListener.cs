using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportationListener : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private List<TeleportationSetting> teleportationSettings;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void CheckStartTeleportation(string log)
    {
        foreach (TeleportationSetting teleportationSetting in teleportationSettings)
        {
            if (teleportationSetting.logToStart == log)
            {
                StartTeleportation(teleportationSetting);
                return;
            }
        }
    }

    private void StartTeleportation(TeleportationSetting teleportationSetting) => PlayerTeleportationManager.Instance.StartTeleportation(teleportationSetting);

    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckStartTeleportation(e.gameplayAction.log);
    }
}
