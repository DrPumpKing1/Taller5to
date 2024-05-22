using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectsDataPersistenceManager))]
public class ObjectsDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectsDataPersistenceManager objectsDataPersistenceManager = (ObjectsDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            objectsDataPersistenceManager.DeleteGameData();
        }
    }
}
