using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPlayerAttachment : MonoBehaviour
{
    public static PetPlayerAttachment Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private string logToInitialAttach;

    [Header("Debug")]
    [SerializeField] private bool overrideAttachToPlayer;
    [SerializeField] private bool attachToPlayer;

    public bool AttachToPlayer => attachToPlayer;

    public static event EventHandler OnVyrxAttachToPlayer;
    public static event EventHandler OnVyrxUnattachToPlayer;
    public static event EventHandler OnVyrxInitialAttachToPlayer;

    public static event EventHandler OnAttachToPlayerConditionsChecked;

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
        CheckAttachConditions();
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

    public void SetInitialAttachToPlayer(bool attach) => initialAttachFromSave = attach; //Called in Awake() by PetPlayerAttachmentPersistence

    private void CheckAttachConditions()
    {
        CheckSetInitialAttachToPlayer();
        SetIntialOverrideAttachToPlayer(attachToPlayer);

        OnAttachToPlayerConditionsChecked?.Invoke(this, EventArgs.Empty);
    }

    private void CheckSetInitialAttachToPlayer()
    {
        if (overrideAttachToPlayer) return;
        SetAttachToPlayer(initialAttachFromSave,true);
    }

    private void SetIntialOverrideAttachToPlayer(bool attach)
    {
        if (!overrideAttachToPlayer) return;
        SetAttachToPlayer(attach, true);
    }

    public void SetAttachToPlayer(bool attach, bool triggerEvents)
    {
        if (attach)
        {
            if(triggerEvents) OnVyrxAttachToPlayer?.Invoke(this, EventArgs.Empty);
            attachToPlayer = true;
        }
        else
        {
            if(triggerEvents) OnVyrxUnattachToPlayer?.Invoke(this, EventArgs.Empty);
            attachToPlayer = false;
        }
    }

    private void InitialAttachToPlayer()
    {
        SetAttachToPlayer(true, true);
        OnVyrxInitialAttachToPlayer?.Invoke(this, EventArgs.Empty);
    }

    #region GameLogManagerSubscriptions
    private void GameLogManager_OnLogAdd(object sender, GameLogManager.OnLogAddEventArgs e)
    {
        if (e.gameplayAction.log != logToInitialAttach) return;
        InitialAttachToPlayer();
    }

    #endregion
}
