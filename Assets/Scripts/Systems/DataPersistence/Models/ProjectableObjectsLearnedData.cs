using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectableObjectsLearnedData
{
    public SerializableDictionary<int, bool> projectableObjectsLearned;

    public ProjectableObjectsLearnedData()
    {
        projectableObjectsLearned = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
    }
}
