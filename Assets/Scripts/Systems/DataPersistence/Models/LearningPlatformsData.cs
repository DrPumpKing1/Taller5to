using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LearningPlatformsData
{
    public SerializableDictionary<int, bool> learningPlatformsUsed;

    public LearningPlatformsData()
    {
        learningPlatformsUsed = new SerializableDictionary<int, bool>(); //String -> id; bool -> isLearned
    }
}
