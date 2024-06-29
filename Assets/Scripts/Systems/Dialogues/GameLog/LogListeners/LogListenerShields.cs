using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerShields : MonoBehaviour
{
    private void OnEnable()
    {
        //SHIELDS
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
        NotEnoughShieldsCollider.OnNotEnoughShieldsCollider += NotEnoughShieldsCollider_OnNotEnoughShieldsCollider;
        CantOpenDoahCollider.OnCantOpenDoahCollider += CantOpenDoahCollider_OnCantOpenDoahCollider;
    }

    private void OnDisable()
    {
        //SHIELDS
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
        NotEnoughShieldsCollider.OnNotEnoughShieldsCollider -= NotEnoughShieldsCollider_OnNotEnoughShieldsCollider;
        CantOpenDoahCollider.OnCantOpenDoahCollider -= CantOpenDoahCollider_OnCantOpenDoahCollider;
    }

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e)
    {
        GameLogManager.Instance.Log($"Worth/ShowDignity/{e.dialect}");
        GameLogManager.Instance.Log($"Worth/ShowDignityExact/{e.dialect}/{e.id}");
    }

    private void NotEnoughShieldsCollider_OnNotEnoughShieldsCollider(object sender, NotEnoughShieldsCollider.OnNotEnoughShieldsColliderEventArgs e) => GameLogManager.Instance.Log($"Worth/NotEnoughShields/{e.dialect}");
    private void CantOpenDoahCollider_OnCantOpenDoahCollider(object sender, CantOpenDoahCollider.OnCantOpenDoahColliderEventArgs e) => GameLogManager.Instance.Log($"Worth/CantOpenDoah/{e.dialect}");
}
