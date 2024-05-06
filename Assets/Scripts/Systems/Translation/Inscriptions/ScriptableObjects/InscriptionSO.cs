using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInscriptionSO", menuName = "ScriptableObjects/Inscriptions/Inscription")]
public class InscriptionSO : ScriptableObject
{
    public int id;
    public Dialect dialect;
    public string title;
    public string text;
    public Sprite image;
}
