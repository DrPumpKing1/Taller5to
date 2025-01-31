using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlayerPersistence : MonoBehaviour, IDataPersistence<PetData>
{
    public void LoadData(PetData data)
    {
        PetPlayerAttachment petPlayerAttachment = FindObjectOfType<PetPlayerAttachment>();

        if (!data.attachToPlayer) return; //If its false, it means PlayerData has been initialized as a new()

        if (data.attachToPlayer) petPlayerAttachment.SetInitialAttachToPlayer(true);
        else petPlayerAttachment.SetInitialAttachToPlayer(false);
    }

    public void SaveData(ref PetData data)
    {
        PetPlayerAttachment petPlayerAttachment = FindObjectOfType<PetPlayerAttachment>();

        data.attachToPlayer = petPlayerAttachment.AttachToPlayer;
    }
}
