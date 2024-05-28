using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueSO", menuName = "ScriptableObjects/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public int id;
    public List<Sentence> sentences;
}
