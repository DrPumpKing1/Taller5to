using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InscriptionsTranslatedPersistence : MonoBehaviour, IDataPersistence<ObjectsData>
{
    public void LoadData(ObjectsData data)
    {
        Inscription[] inscriptions = FindObjectsOfType<Inscription>();

        foreach (Inscription inscription in inscriptions)
        {
            foreach (KeyValuePair<int, bool> inscriptionTranslated in data.inscriptionsTranslated)
            {
                if (inscription.ID == inscriptionTranslated.Key)
                {
                    if (inscriptionTranslated.Value) inscription.SetIsTranslated();
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
            if (data.inscriptionsTranslated.ContainsKey(inscription.ID)) data.inscriptionsTranslated.Remove(inscription.ID);
        }

        foreach (Inscription inscription in inscriptions)
        {
            bool translated = inscription.IsTranslated;

            data.inscriptionsTranslated.Add(inscription.ID, translated);
        }
    }
}