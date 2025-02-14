using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class GeneralDataMethods
{
    private const string ACHIEVEMENTS_DATA_PATH = "achievementsData.wwdata";

    public static void DeleteDataInPaths(IEnumerable<string> dataPaths)
    {
        foreach (string dataPath in dataPaths)
        {
            DeleteDataInPath(dataPath);
        }
    }

    public static void DeleteDataInPathsExceptAchievements(IEnumerable<string> dataPaths)
    {
        foreach (string dataPath in dataPaths)
        {
            if (dataPath == ACHIEVEMENTS_DATA_PATH) continue;
            DeleteDataInPath(dataPath);
        }
    }

    public static void DeleteDataInPath(string dataPath)
    {
        string dirPath = Application.persistentDataPath;

        string path = Path.Combine(dirPath, dataPath);

        if (!File.Exists(path))
        {
            Debug.Log("No data to delete");
        }
        else
        {
            File.Delete(path);
            Debug.Log("Data Deleted");
        }
    }

    public static bool CheckIfDataPathsExist(IEnumerable<string> dataPaths)
    {
        foreach (string dataPath in dataPaths)
        {
            if (!CheckIfDataPathExists(dataPath)) return false;
        }

        return true;
    }


    public static bool CheckIfDataPathExists(string dataPath)
    {
        string dirPath = Application.persistentDataPath;
        string path = Path.Combine(dirPath, dataPath);

        if (File.Exists(path)) return true;

        return false;
    }
}
