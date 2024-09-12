using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewJournalInfoSO", menuName = "ScriptableObjects/JournalInfo")]
public class JournalInfoSO : ScriptableObject
{
    public int id;
    public string infoName;
}
