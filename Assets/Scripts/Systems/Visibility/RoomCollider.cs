using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollider : MonoBehaviour
{
    [Header("Identifiers")]
    [SerializeField] private int id;
    [SerializeField] private string roomName;

    public int ID => id;
    public string RoomName => roomName;
}
