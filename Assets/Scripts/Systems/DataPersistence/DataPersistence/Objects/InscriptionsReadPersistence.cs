using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InscriptionsReadPersistence : MonoBehaviour, IDataPersistence<ObjectsData>
{
    public void LoadData(ObjectsData data)
    {
        Inscription[] inscriptions = FindObjectsOfType<Inscription>();

        foreach (Inscription inscription in inscriptions)
        {
            foreach (KeyValuePair<int, bool> inscriptionRead in data.inscriptionsRead)
            {
                if (inscription.InscriptionSO.id == inscriptionRead.Key)
                {
                    if (inscriptionRead.Value) inscription.SetHasBeenRead(true);
                    else inscription.SetHasBeenRead(false);
                    break;
                }
            }
        }
    }

    public void SaveData(ref ObjectsData data)
    {
        Inscription[] inscriptions = FindObjectsOfType<Inscription>();

        foreach (Inscription inscription in inscriptions)
        {
            if (data.inscriptionsRead.ContainsKey(inscription.InscriptionSO.id)) data.inscriptionsRead.Remove(inscription.InscriptionSO.id);
        }

        foreach (Inscription inscription in inscriptions)
        {
            bool hasBeenRead = inscription.HasBeenRead;

            data.inscriptionsRead.Add(inscription.InscriptionSO.id, hasBeenRead);
        }
    }
}
