using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/Audio/MusicPool")]
public class MusicPoolSO : ScriptableObject
{
    public AudioClip gamePlayMusic;
    public AudioClip menuMusic;
    public AudioClip creditsMusic;

}
