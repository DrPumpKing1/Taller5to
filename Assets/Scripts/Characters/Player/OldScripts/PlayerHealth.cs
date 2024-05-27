using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isDead;

    public bool IsDead => isDead;

    public static event EventHandler OnPlayerDeath;
    
    private void OnEnable()
    {
        //Subscribe to traps or in general any event that will kill the player to KillPlayer()
    }

    private void OnDisable()
    {
        //Unsubscribe to traps or in general any event that will kill the player to KillPlayer()
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) //Test
        {
            KillPlayer();
        }
    }

    private void InitializeVariables()
    {
        isDead = false;
    }

    private void KillPlayer()
    {
        if (isDead) return;

        isDead = true;
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }
}
