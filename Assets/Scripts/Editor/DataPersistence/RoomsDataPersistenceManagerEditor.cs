using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomsDataPersistenceManager))]
public class RoomsDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoomsDataPersistenceManager RoomsDataPersistenceManager = (RoomsDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            RoomsDataPersistenceManager.DeleteGameData();
        }
    }
}
