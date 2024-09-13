using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JournalDataPersistenceManager))]
public class JournalDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        JournalDataPersistenceManager journalDataPersistenceManager = (JournalDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            journalDataPersistenceManager.DeleteGameData();
        }
    }
}
