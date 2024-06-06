using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/SFXPool")]
public class SFXPoolSO : ScriptableObject
{
    public AudioClip[] objectProjected;
    public AudioClip[] objectFailedProjection;

    public AudioClip[] objectDematerialization;

    public AudioClip[] doorEnergized;
    public AudioClip[] doorDeEnergized;

    public AudioClip[] switchToggle;

    public AudioClip[] shieldCollected;
    public AudioClip[] valueDoorOpened;
}
