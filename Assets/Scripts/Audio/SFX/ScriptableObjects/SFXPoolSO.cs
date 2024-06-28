using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/Audio/SFXPool")]
public class SFXPoolSO : ScriptableObject
{
    [Header("Player")]
    public AudioClip[] playerLand;
    public AudioClip[] vyrxAttach;

    [Header("Learning")]
    public AudioClip[] objectLearned;

    [Header("Exact Object Learning")]
    public AudioClip[] cableLearned;
    public AudioClip[] magicBoxLearned;
    public AudioClip[] senderLearned;
    public AudioClip[] drainerLearned;

    [Header("Projection")]
    public AudioClip[] objectProjected;
    public AudioClip[] objectFailedProjection;
    public AudioClip[] objectDematerialization;
    public AudioClip[] projectionResetObjectUsed;

    [Header("Exact Object Projection")]
    public AudioClip[] cableProjected;
    public AudioClip[] magicBoxProjected;
    public AudioClip[] senderProjected;
    public AudioClip[] drainerProjected;

    [Header("Exact Object Dematerialization")]
    public AudioClip[] cableDematerialized;
    public AudioClip[] magicBoxDematerialized;
    public AudioClip[] senderDematerialized;
    public AudioClip[] drainerDematerialized;

    [Header("Object Interaction")]
    public AudioClip[] objectRotated;
    public AudioClip[] objectActivation;
    public AudioClip[] objectDeactivation;

    [Header("Exact Object Interaction")]
    public AudioClip[] cableRotated;
    public AudioClip[] senderRotated;
    public AudioClip[] magicBoxActivated;
    public AudioClip[] magicBoxDeactivated;
    public AudioClip[] drainerActivated;
    public AudioClip[] drainerDeactivated;

    [Header("Projectiles")]
    public AudioClip[] projectileShot;
    public AudioClip[] projectileImpact;

    [Header("Shields")]
    public AudioClip[] shieldCollected;
    public AudioClip[] valueDoorOpened;

    [Header("Exact Shields")]
    public AudioClip[] zurrythShieldCollected;
    public AudioClip[] rakithuShieldCollected;
    public AudioClip[] xotarkShieldCollected;
    public AudioClip[] vythanuShieldCollected;

    [Header("Exact Value Doors")]
    public AudioClip[] zurrythValueDoorOpened;
    public AudioClip[] rakithuValueDoorOpened;
    public AudioClip[] xotarkValueDoorOpened;
    public AudioClip[] vythanuValueDoorOpened;

    [Header("Electrical")]
    public AudioClip[] switchToggle;
    public AudioClip[] switchOn;
    public AudioClip[] switchOff;

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
