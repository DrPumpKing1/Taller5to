using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerDataPersistenceManager))]
public class PlayerDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerDataPersistenceManager playerDataPersistenceManager = (PlayerDataPersistenceManager)target;

        if(GUILayout.Button("Delete Data File"))
        {
            playerDataPersistenceManager.DeleteGameData();
        }
    }
}
