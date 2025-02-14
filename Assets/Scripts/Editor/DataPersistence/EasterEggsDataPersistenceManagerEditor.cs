using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EasterEggsDataPersistenceManager))]
public class EasterEggsDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EasterEggsDataPersistenceManager EasterEggsDataPersistenceManager = (EasterEggsDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            EasterEggsDataPersistenceManager.DeleteGameData();
        }
    }
}
