using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class DataPersistenceManager<T> : MonoBehaviour where T : class, new()
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private string dirPath;
    private IDataService dataService;
    private T persistentData;
    private List<IDataPersistence<T>> dataPersistenceObjects;


    protected void Awake()
    {
        SetSingleton();
        InitializeDataPersistenceManager();
    }

    protected void InitializeDataPersistenceManager()
    {
        dirPath = Application.persistentDataPath;

        if (useEncryption) dataService = new JSONNewtonSoftDataServiceEncryption(dirPath, fileName);
        else dataService = new JSONNewtonsoftDataServiceNoEncryption(dirPath, fileName);

        dataPersistenceObjects = FindAllDataPersistencesObjects();
        LoadGameData();
    }


    protected List<IDataPersistence<T>> FindAllDataPersistencesObjects()
    {
        IEnumerable<IDataPersistence<T>> dataPersistenceObjectsNumerable = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence<T>>();
        List<IDataPersistence<T>> dataPersistenceObjectsList = new List<IDataPersistence<T>>(dataPersistenceObjectsNumerable);

        return dataPersistenceObjectsList;
    }

    protected virtual void SetSingleton() { }

    protected void LoadGameData()
    {
        persistentData = dataService.LoadData<T>(); //Load data from file using data handler

        if (persistentData == default || persistentData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGameData();
        }

        foreach (IDataPersistence<T> dataPersistenceObject in dataPersistenceObjects) //Push loaded data to scripts that need it
        {
            dataPersistenceObject.LoadData(persistentData);
        }
    }

    protected void SaveGameData()
    {
        foreach (IDataPersistence<T> dataPersistenceObject in dataPersistenceObjects) //Pass data to other scripts so they can update it
        {
            dataPersistenceObject.SaveData(ref persistentData);
        }

        dataService.SaveData(persistentData); //Save data to file using data handler 
    }

    protected void NewGameData() => persistentData = new T();

    private void OnApplicationQuit() => SaveGameData();

}
