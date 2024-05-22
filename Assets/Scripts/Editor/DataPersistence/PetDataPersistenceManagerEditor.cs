using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PetDataPersistenceManager))]
public class PetDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PetDataPersistenceManager petDataPersistenceManager = (PetDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            petDataPersistenceManager.DeleteGameData();
        }
    }
}
