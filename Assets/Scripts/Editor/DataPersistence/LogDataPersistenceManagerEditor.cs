using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LogDataPersistenceManager))]
public class LogDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LogDataPersistenceManager logDataPersistenceManager = (LogDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            logDataPersistenceManager.DeleteGameData();
        }
    }
}
