using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/Audio/SFXPool")]
public class SFXPoolSO : ScriptableObject
{
    public AudioClip[] objectProjected;
    public AudioClip[] objectFailedProjection;

    public AudioClip[] objectDematerialization;
    public AudioClip[] projectionResetObjectUsed;

    public AudioClip[] objectRotated;

    public AudioClip[] objectActivation;
    public AudioClip[] objectDeactivation;

    public AudioClip[] shieldCollected;
    public AudioClip[] valueDoorOpened;

    public AudioClip[] switchToggle;

    public AudioClip[] doorPowered;
    public AudioClip[] doorDePowered;

    public AudioClip[] drawbridgePowered;
    public AudioClip[] drawbridgeDePowered;

    public AudioClip[] extensibleBridgePowered;
    public AudioClip[] extensibleBridgeDePowered;

    public AudioClip[] inscriptionPowered;
    public AudioClip[] inscriptionDePowered;

    public AudioClip[] receiverPowered;
    public AudioClip[] receiverDePowered;
}
