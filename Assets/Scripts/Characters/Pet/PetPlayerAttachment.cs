using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPlayerAttachment : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool attachToPlayer;
    public bool AttachToPlayer => attachToPlayer;

    private void Awake()
    {
        IgnorePetPlayerCollisions();
    }

    private void IgnorePetPlayerCollisions() => Physics.IgnoreLayerCollision(6, 8);

    public void SetAttachToPlayer() => attachToPlayer = true;
}
