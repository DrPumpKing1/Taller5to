using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPlayerAttachment : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool attachToPlayer;

    public bool AttachToPlayer => attachToPlayer;
}
