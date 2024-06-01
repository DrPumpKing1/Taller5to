using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPlayerAttachment : MonoBehaviour
{
    public static PetPlayerAttachment Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private bool attachToPlayer;
    public bool AttachToPlayer => attachToPlayer;

    private void OnEnable()
    {
        MeetVyrxEnd.OnMeetVyrxEnd += MeetVyrxEnd_OnMeetVyrxEnd;
    }

    private void OnDisable()
    {
        MeetVyrxEnd.OnMeetVyrxEnd -= MeetVyrxEnd_OnMeetVyrxEnd;
    }

    private void Awake()
    {
        IgnorePetPlayerCollisions();
        SetSingleton();
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

    public void SetAttachToPlayer(bool attach) => attachToPlayer = attach;

    private void MeetVyrxEnd_OnMeetVyrxEnd(object sender, EventArgs e) => SetAttachToPlayer(true);

}
