using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeListener : MonoBehaviour
{
    [Header("Virtue Door Settings")]
    [SerializeField,Range(0,1f)] private float virtueDoorShakeAmplitude;
    [SerializeField,Range(0,5f)] private float virtueDoorShakeFrequency;
    [SerializeField,Range(0.5f,10f)] private float virtueDoorShakeTime;
    [SerializeField,Range(0f,10f)] private float virtueDoorShakeFadeInTime;
    [SerializeField,Range(0f,10f)] private float virtueDoorShakeFadeOutTime;

    private void OnEnable()
    {
        ShieldDoor.OnShieldDoorOpen += ShieldDoor_OnShieldDoorOpen;
    }

    private void OnDisable()
    {
        ShieldDoor.OnShieldDoorOpen -= ShieldDoor_OnShieldDoorOpen;
    }

    private void ShieldDoor_OnShieldDoorOpen(object sender, ShieldDoor.OnShieldDoorOpenEventArgs e)
    {
        CameraShakeHandler.Instance.ShakeCamera(virtueDoorShakeAmplitude, virtueDoorShakeFrequency, virtueDoorShakeTime, virtueDoorShakeFadeInTime, virtueDoorShakeFadeOutTime);
    }
}
