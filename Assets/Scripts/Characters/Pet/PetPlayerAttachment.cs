using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPlayerAttachment : MonoBehaviour
{
    public static PetPlayerAttachment Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private string logToAttach;

    [Header("Debug")]
    [SerializeField] private bool overrideAttachToPlayer;
    [SerializeField] private bool attachToPlayer;

    public bool AttachToPlayer => attachToPlayer;

    public static event EventHandler OnVyrxAttachToPlayer;
    public static event EventHandler OnVyrxUnattachToPlayer;
    public static event EventHandler OnVyrxInitialAttachToPlayer;

    private bool initialAttachFromSave;

    private void OnEnable()
    {
        GameLogManager.OnLogAdd += GameLogManager_OnLogAdd;
    }

    private void OnDisable()
    {
        GameLogManager.OnLogAdd -= GameLogManager_OnLogAdd;
    }

    private void Awake()
    {
        IgnorePetPlayerCollisions();
        SetSingleton();      
    }

    private void Start()
    {
        CheckSetInitialAttachToPlayer();
        SetIntialOverrideAttachToPlayer(attachToPlayer);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one PetPlayerAttachment instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void IgnorePetPlayerCollisions() => Physics.IgnoreLayerCollision(6, 8);

    public void SetInitialAttachToPlayer(bool attach) => initialAttachFromSave = attach;

    private void CheckSetInitialAttachToPlayer()
    {
        if (overrideAttachToPlayer) return;
        SetAttachToPlayer(initialAttachFromSave);
    }

    private void SetIntialOverrideAttachToPlayer(bool attach)
    {
        if (!overrideAttachToPlayer) return;
        SetAttachToPlayer(attach);
    }

    public void SetAttachToPlayer(bool attach)
    {
        if (attach)
        {
            OnVyrxAttachToPlayer?.Invoke(this, EventArgs.Empty);
            attachToPlayer = true;
        }
        else
        {
            OnVyrxUnattachToPlayer?.Invoke(this, EventArgs.Empty);
            attachToPlayer = false;
        }
    }

    #region GameLogManagerSubscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToAttach) return;
        SetAttachToPlayer(true);
        OnVyrxInitialAttachToPlayer?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
