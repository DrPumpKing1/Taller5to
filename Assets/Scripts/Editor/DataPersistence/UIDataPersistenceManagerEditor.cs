using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIDataPersistenceManager))]
public class UIDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UIDataPersistenceManager UIDataPersistenceManager = (UIDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            UIDataPersistenceManager.DeleteGameData();
        }
    }
}
