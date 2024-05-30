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
                if (learningPlatform.LearningPlatformSO.id == learningPlatformUsed.Key)
                {
                    if (learningPlatformUsed.Value) learningPlatform.SetIsLearned(true);
                    else learningPlatform.SetIsLearned(false);
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
            if (data.learningPlatformsUsed.ContainsKey(learningPlatform.LearningPlatformSO.id)) data.learningPlatformsUsed.Remove(learningPlatform.LearningPlatformSO.id);
        }

        foreach (LearningPlatform learningPlatform in learningPlatforms)
        {
            bool learned = learningPlatform.IsLearned;

            data.learningPlatformsUsed.Add(learningPlatform.LearningPlatformSO.id, learned);
        }
    }
}
