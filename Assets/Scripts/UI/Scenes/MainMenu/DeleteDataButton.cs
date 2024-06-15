using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DeleteDataButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button deleteDataButton;

    [Header("Data Paths")]
    [SerializeField] private List<string> dataPaths;

    private void Awake()
    {
        InitializeButtonListeners();
    }

    private void InitializeButtonListeners()
    {
        deleteDataButton.onClick.AddListener(DeleteData);
    }

    private void DeleteData()
    {
        string dirPath = Application.persistentDataPath;

        foreach(string dataPath in dataPaths)
        {
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
    }
}
