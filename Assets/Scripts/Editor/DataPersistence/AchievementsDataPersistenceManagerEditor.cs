using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AchievementsDataPersistenceManager))]
public class AchievementsDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AchievementsDataPersistenceManager achievementsDataPersistenceManager = (AchievementsDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            achievementsDataPersistenceManager.DeleteGameData();
        }
    }
}
