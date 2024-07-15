using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public SpeakerSO speaker;
    [TextArea(3,10)] public string text;
    public float time;
    public AudioClip audioClip;
}
