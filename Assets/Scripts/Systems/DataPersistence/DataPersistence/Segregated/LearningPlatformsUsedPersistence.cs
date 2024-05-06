using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningPlatformsUsedPersistence : MonoBehaviour, IDataPersistence<ObjectsData>
{
    public void LoadData(ObjectsData data)
    {
        LearningPlatform[] learningPlatforms = FindObjectsOfType<LearningPlatform>();

        foreach (LearningPlatform learningPlatform in learningPlatforms)
        {
            foreach (KeyValuePair<int, bool> learningPlatformUsed in data.learningPlatformsUsed)
            {
                if (learningPlatform.ID == learningPlatformUsed.Key)
                {
                    if (learningPlatformUsed.Value) learningPlatform.SetIsLearned();
                    break;
                }
            }
        }
    }

    public void SaveData(ref ObjectsData data)
    {
        LearningPlatform[] learningPlatforms = FindObjectsOfType<LearningPlatform>();

        foreach (LearningPlatform learningPlatform in learningPlatforms)
        {
            if (data.learningPlatformsUsed.ContainsKey(learningPlatform.ID)) data.learningPlatformsUsed.Remove(learningPlatform.ID);
        }

        foreach (LearningPlatform learningPlatform in learningPlatforms)
        {
            bool learned = learningPlatform.IsLearned;

            data.learningPlatformsUsed.Add(learningPlatform.ID, learned);
        }
    }
}
