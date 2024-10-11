using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueSO", menuName = "ScriptableObjects/Dialogues/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public int id;
    public MovementLimitType movementLimitType;
    public bool allowSkip;
    public bool lookAtPlayer;
    public bool lookAtPet;
    public List<Sentence> sentences;
}

public enum MovementLimitType {FreeMovement,RestrictedMovement,ZeroMovement}
