using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/SFXPool")]
public class SFXPoolSO : ScriptableObject
{
    public AudioClip[] objectProjected;
    public AudioClip[] doorEnergized;
    public AudioClip[] doorDeEnergized;
    public AudioClip[] shieldColledted;
    public AudioClip[] valueDoorOpened;
    public AudioClip[] switchToggle;
}
