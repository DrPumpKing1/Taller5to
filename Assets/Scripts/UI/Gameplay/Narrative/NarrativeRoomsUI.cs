using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeRoomsUI : MonoBehaviour
{
    private void OnEnable()
    {
        NarrativeRoomsManager.OnNarrativeRoomVisited += NarrativeRoomsManager_OnNarrativeRoomVisited;
    }
    private void OnDisable()
    {
        NarrativeRoomsManager.OnNarrativeRoomVisited -= NarrativeRoomsManager_OnNarrativeRoomVisited;
    }

    private void NarrativeRoomsManager_OnNarrativeRoomVisited(object sender, NarrativeRoomsManager.OnNarrativeRoomEventArgs e)
    {
        //EnableFullScreenShader
    }
}
