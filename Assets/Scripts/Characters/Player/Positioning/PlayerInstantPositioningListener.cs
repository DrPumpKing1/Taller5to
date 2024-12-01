using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstantPositioningListener : MonoBehaviour
{
    [Header("Guidance Settings")]
    [SerializeField] private List<PlayerInstantPositionObject> instantPositionObjects;

    [System.Serializable]
    public class PlayerInstantPositionObject
    {
        public int id;
        public string instantPositionName;
        public string logToInstantPosition;
        public Transform positionTransform;
    }

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void CheckInstantPosition(string log)
    {
        foreach (PlayerInstantPositionObject playerInstantPositionObject in instantPositionObjects)
        {
            if (playerInstantPositionObject.logToInstantPosition == log)
            {
                PlayerPositioningHandler.Instance.InstantPositionPlayer(playerInstantPositionObject.positionTransform.position);
                return;
            }
        }
    }

    #region GameLogManager Subscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        CheckInstantPosition(e.gameplayAction.log);
    }
    #endregion
}
