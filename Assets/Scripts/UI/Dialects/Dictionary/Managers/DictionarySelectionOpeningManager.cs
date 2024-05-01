using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DictionarySelectionOpeningManager : MonoBehaviour
{
    public static DictionarySelectionOpeningManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    public static event EventHandler OnDictionarySelectionOpen;
    public static event EventHandler OnDictionarySelectionClose;

    private bool DictionaryInput => UIInput.GetDictionaryDown();

    public bool DictionarySelectionOpen { get; private set; }
    public bool DictionarySelectionOpenedThisFrame { get; private set; }

    private void OnEnable()
    {
        DictionarySelectionUI.OnCloseFromUI += DictionarySelectionUI_OnCloseFromUI;
    }

    private void OnDisable()
    {
        DictionarySelectionUI.OnCloseFromUI -= DictionarySelectionUI_OnCloseFromUI;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        CheckOpenCloseDictionary();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one DictionarySelectionOpeningManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        DictionarySelectionOpen = false;
        DictionarySelectionOpenedThisFrame = false;
    }
    private void CheckOpenCloseDictionary()
    {
        DictionarySelectionOpenedThisFrame = false;

        if (!DictionaryInput) return;

        if (!DictionarySelectionOpen)
        {
            if (UIManager.Instance.UIActive) return;
            OpenDictionarySelection();
            return;
        }
        else    
        {
            if (UIManager.Instance.GetUILayersCount() == 1) //If count is 1, the active layer is the DictionaryUI
            CloseDictionarySelection();
            return;
        }
    }

    private void OpenDictionarySelection()
    {
        OnDictionarySelectionOpen?.Invoke(this, EventArgs.Empty);
        DictionarySelectionOpen = true;
    }

    private void CloseDictionarySelection()
    {
        OnDictionarySelectionClose?.Invoke(this, EventArgs.Empty);
        DictionarySelectionOpen = false;
    }

    #region DictionarySelectionUI Subscriptions

    private void DictionarySelectionUI_OnCloseFromUI(object sender, EventArgs e)
    {
        CloseDictionarySelection();
    }
    #endregion
}
