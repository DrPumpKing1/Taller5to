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
    [SerializeField] private DataPathsSO dataPathsSO;

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
        GeneralDataMethods.DeleteDataInPaths(dataPathsSO.dataPaths);
    }
}
