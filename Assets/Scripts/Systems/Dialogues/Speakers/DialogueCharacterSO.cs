using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpeakerSO", menuName = "ScriptableObjects/Speaker")]
public class SpeakerSO : ScriptableObject
{
    public int id;
    public string speakerName;
    public Sprite speakerImage;
    public Color nameColor;
}
