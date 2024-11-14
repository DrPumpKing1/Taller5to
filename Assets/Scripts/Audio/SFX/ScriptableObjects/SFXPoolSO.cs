using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/Audio/SFXPool")]
public class SFXPoolSO : ScriptableObject
{
    [Header("Player")]
    public AudioClip playerWalk;
    public AudioClip playerSprint;

    public AudioClip[] playerLand;
    public AudioClip[] vyrxAttach;

    [Header("Learning")]
    public AudioClip startLearning;
    public AudioClip[] objectLearned;

    [Header("Exact Object Learning")]
    public AudioClip[] cableLearned;
    public AudioClip[] magicBoxLearned;
    public AudioClip[] senderLearned;
    public AudioClip[] drainerLearned;

    [Header("Projectable Object Selection")]
    public AudioClip[] objectSelected;

    [Header("Exact Projectable Object Selection")]
    public AudioClip[] cableSelected;
    public AudioClip[] magicBoxSelected;
    public AudioClip[] senderSelected;
    public AudioClip[] drainerSelected;

    [Header("Projection")]
    public AudioClip startProjection;
    public AudioClip[] objectProjected;
    public AudioClip[] objectFailedProjection;
    public AudioClip startDematerialization;
    public AudioClip[] objectDematerialization;
    public AudioClip startDematerializationAll;
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

    [Header("Dialogues")]
    public AudioClip[] dialogueOpen;
    public AudioClip[] dialogueClose;

    [Header("Monologues")]
    public AudioClip[] monologueOpen;
    public AudioClip[] monologueClose;

    [Header("Boss")]
    public AudioClip bossBeamSphereStart;
    public AudioClip[] bossBeamSphereCast;
    public AudioClip[] bossBeamSphereFade;
    public AudioClip[] bossBeamSphereTargetLocked;
    public AudioClip[] bossDematerializationLightning;
    public AudioClip[] bossShieldActivated;
    public AudioClip[] bossShieldDeactivated;
    public AudioClip[] bossNextPhase;
    public AudioClip[] bossAlmostDefeated;
    public AudioClip[] bossDefeated;

    [Header("Showcase Rooms")]
    public AudioClip showcaseRoomBeamSphereStart;
    public AudioClip[] showcaseRoomBeamSphereCast;
    public AudioClip[] showcaseRoomBeamSphereFade;
    public AudioClip[] showcaseRoomBeamSphereTargetLocked;
    public AudioClip[] showcaseRoomDematerializationLightning;
    public AudioClip[] showcaseRoomShieldActivated;
    public AudioClip[] showcaseRoomShieldDeactivated;
    public AudioClip[] showcaseRoomNextPhase;

    [Header("Ancient Relic")]
    public AudioClip[] ancientRelicShieldDepowered;

    [Header("UI")]
    public AudioClip[] pauseOpen;
    public AudioClip[] pauseClose;
    [Space]
    public AudioClip[] inventoryOpen;
    public AudioClip[] inventoryClose;
    [Space]
    public AudioClip[] journalOpen;
    public AudioClip[] journalClose;
    public AudioClip[] journalInfoCollected;
    [Space]
    public AudioClip[] achievementAchieved;
    [Space]
    public AudioClip[] buttonClick1;
    public AudioClip[] buttonClick2;
    public AudioClip[] buttonClick3;
}
