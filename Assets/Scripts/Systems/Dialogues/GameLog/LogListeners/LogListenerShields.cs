using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerShields : MonoBehaviour
{
    private void OnEnable()
    {
        //SHIELDS
        ShieldPiecesManager.OnShieldPieceCollected += ShieldPiecesManager_OnShieldPieceCollected;
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
    }

    private void OnDisable()
    {
        //SHIELDS
        ShieldPiecesManager.OnShieldPieceCollected -= ShieldPiecesManager_OnShieldPieceCollected;
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
    }

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e)
    {
        GameLogManager.Instance.Log($"Shields/OpenDoor/{e.dialect}");
        GameLogManager.Instance.Log($"Shields/OpenDoorExact/{e.dialect}/{e.id}");
    }

    private void ShieldPiecesManager_OnShieldPieceCollected(object sender, ShieldPiecesManager.OnShieldPieceCollectedEventArgs e) => GameLogManager.Instance.Log($"Shields/CollectShieldPiece/{e.shieldPieceSO.id}");
}
