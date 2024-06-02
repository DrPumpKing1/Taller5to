using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerShields : MonoBehaviour
{
    private void OnEnable()
    {
        //SHIELDS
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
    }

    private void OnDisable()
    {
        //SHIELDS
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
    } 

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e) => GameLogManager.Instance.Log($"Worth/ShowDignity/{e.dialect}");
}
