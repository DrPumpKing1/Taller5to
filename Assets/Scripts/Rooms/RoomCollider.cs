using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollider : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;
    [SerializeField] private string roomName;
    [SerializeField] private MusicLevel musicLevel;
    [SerializeField] private TitleLevel titleLevel;

    public int ID => id;
    public string RoomName => roomName;
    public MusicLevel MusicLevel => musicLevel;
    public TitleLevel TitleLevel => titleLevel;
}
