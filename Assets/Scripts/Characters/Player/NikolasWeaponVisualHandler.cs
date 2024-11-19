using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NikolasWeaponVisualHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform model;

    private void OnEnable()
    {
        PetPlayerAttachment.OnVyrxAttachToPlayer += PetPlayerAttachment_OnVyrxAttachToPlayer;
    }

    private void OnDisable()
    {
        PetPlayerAttachment.OnVyrxAttachToPlayer -= PetPlayerAttachment_OnVyrxAttachToPlayer;
    }

    private void SetModel(bool enable) => model.gameObject.SetActive(enable);

    private void PetPlayerAttachment_OnVyrxAttachToPlayer(object sender, System.EventArgs e)
    {
        SetModel(true);
    }
}
