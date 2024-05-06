using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class JSONUtilityDataServiceNoEncryption : IDataService
{
    private string dirPath;
    private string filePath;

    public JSONUtilityDataServiceNoEncryption(string dirPath, string filePath)
    {
        this.dirPath = dirPath;
        this.filePath = filePath;
    }
    
    public bool SaveData<T>(T data)
    {
        string path = Path.Combine(dirPath, filePath);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string dataToStore = JsonUtility.ToJson(data, true);

            using FileStream stream = new FileStream(path, FileMode.Create);
            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(dataToStore);

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public T LoadData<T>()
    {
        string path = Path.Combine(dirPath, filePath);

        T loadedData = default;

        if (File.Exists(path))
        {
            try
            {
                string dataToLoad = "";
                using FileStream stream = new FileStream(path, FileMode.Open);
                using StreamReader reader = new StreamReader(stream);
                dataToLoad = reader.ReadToEnd();

                loadedData = JsonUtility.FromJson<T>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
                throw e;
            }
        }

        return loadedData;
    }
}
