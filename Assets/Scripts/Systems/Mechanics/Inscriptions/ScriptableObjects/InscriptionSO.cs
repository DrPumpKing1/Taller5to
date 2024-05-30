using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInscriptionSO", menuName = "ScriptableObjects/Inscription")]
public class InscriptionSO : ScriptableObject
{
    public int id;
    public string inscriptionName;
    public Dialect dialect;
}
