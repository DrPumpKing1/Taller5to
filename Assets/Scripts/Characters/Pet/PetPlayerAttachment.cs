using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPlayerAttachment : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool attachToPlayer;

    public bool AttachToPlayer { get; private set;}

    private void Update()
    {
        AttachToPlayer = attachToPlayer;
    }
}
