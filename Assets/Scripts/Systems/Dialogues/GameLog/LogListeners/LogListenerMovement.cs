using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListenerMovement : MonoBehaviour
{
    private void OnEnable()
    {
        //ROOMS
        RoomManager.OnRoomEnter += RoomManager_OnRoomEnter;
        RoomManager.OnRoomExit += RoomManager_OnRoomExit;
    }

    private void OnDisable()
    {
        //INTERACTION
        RoomManager.OnRoomEnter -= RoomManager_OnRoomEnter;
        RoomManager.OnRoomExit -= RoomManager_OnRoomExit;
    }

    private void RoomManager_OnRoomEnter(object sender, RoomManager.OnRoomEventArgs e) => GameLogManager.Instance.Log($"Movement/EnterRoom/{e.roomName}");
    private void RoomManager_OnRoomExit(object sender, RoomManager.OnRoomEventArgs e) => GameLogManager.Instance.Log($"Movement/ExitRoom/{e.roomName}");


}
