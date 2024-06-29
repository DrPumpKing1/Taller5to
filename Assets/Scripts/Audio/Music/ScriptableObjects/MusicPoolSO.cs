using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSFXPoolSO", menuName = "ScriptableObjects/Audio/MusicPool")]
public class MusicPoolSO : ScriptableObject
{
    [Header("Scenes")]
    public AudioClip menuMusic;
    public AudioClip creditsMusic;

    [Header("Tutorial")]
    public AudioClip zurryth0;
    public AudioClip zurryth1;

    [Header("Lobby")]
    public AudioClip lobby;

    [Header("Rakithu")]
    public AudioClip rakithu0;
    public AudioClip rakithu1;
    public AudioClip rakithu2;
    public AudioClip rakithu3;
    public AudioClip rakithu4;

    [Header("Xotark")]
    public AudioClip xotark0;
    public AudioClip xotark1;
    public AudioClip xotark2;
    public AudioClip xotark3;
    public AudioClip xotark4;

    [Header("Vythanu")]
    public AudioClip vythanu0;
    public AudioClip vythanu1;
    public AudioClip vythanu2;
    public AudioClip vythanu3;
    public AudioClip vythanu4;

    [Header("Boss")]
    public AudioClip boss;
    public AudioClip afterBoss;
}
