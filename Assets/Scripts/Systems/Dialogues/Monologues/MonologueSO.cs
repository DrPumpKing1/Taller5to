using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonologueSO", menuName = "ScriptableObjects/Dialogues/Monologue")]
public class MonologueSO : ScriptableObject
{
    public int id;
    public List<Sentence> sentences;
}
