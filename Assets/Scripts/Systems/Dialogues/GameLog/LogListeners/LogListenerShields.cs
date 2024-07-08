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
        GameLogManager.Instance.Log($"Shields/DoorOpen/{e.dialect}");
        GameLogManager.Instance.Log($"Shields/DoorOpenExact/{e.dialect}/{e.id}");
    }

    private void ShieldPiecesManager_OnShieldPieceCollected(object sender, ShieldPiecesManager.OnShieldPieceCollectedEventArgs e) => GameLogManager.Instance.Log($"Shields/CollectShieldPiece/{e.shieldPieceSO.id}");
    private void NotEnoughShieldsCollider_OnNotEnoughShieldsCollider(object sender, NotEnoughShieldsCollider.OnNotEnoughShieldsColliderEventArgs e) => GameLogManager.Instance.Log($"Worth/NotEnoughShields/{e.dialect}");
    private void CantOpenDoahCollider_OnCantOpenDoahCollider(object sender, CantOpenDoahCollider.OnCantOpenDoahColliderEventArgs e) => GameLogManager.Instance.Log($"Worth/CantOpenDoah/{e.dialect}");
}
