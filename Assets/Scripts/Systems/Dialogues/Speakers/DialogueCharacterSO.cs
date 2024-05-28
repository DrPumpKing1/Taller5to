using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpeakerSO", menuName = "ScriptableObjects/Speaker")]
public class SpeakerSO : ScriptableObject
{
    public int id;
    public string characterName;
    public Sprite characterImage;
    public Color nameColor;
}
