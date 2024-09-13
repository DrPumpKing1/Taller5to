using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class JournalInfoContentUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler    
{
    [Header("Components")]
    [SerializeField] private JournalInfoUI journalInfoUI;
    [SerializeField] private GameObject contentGameObject;
    [SerializeField] private GameObject notCollectedGameObject;
    [SerializeField] private GameObject notCheckedIndicator;

    public event EventHandler OnMouseEnterContent;
    public event EventHandler OnMouseExitContent;

    private void OnEnable()
    {
        journalInfoUI.OnJournalInfoCollected += JournalInfoUI_OnJournalInfoCollected;
        journalInfoUI.OnJournalInfoChecked += JournalInfoUI_OnJournalInfoChecked;
    }

    private void OnDisable()
    {
        journalInfoUI.OnJournalInfoCollected += JournalInfoUI_OnJournalInfoCollected;
        journalInfoUI.OnJournalInfoChecked += JournalInfoUI_OnJournalInfoChecked;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterContent?.Invoke(this, EventArgs.Empty);
        Debug.Log("Enter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitContent?.Invoke(this, EventArgs.Empty);
        Debug.Log("Exit");
    }

    private void CollectJournalInfo()
    {
        contentGameObject.SetActive(true);
        notCollectedGameObject.SetActive(false);
        notCheckedIndicator.SetActive(true);
    }

    private void CheckJournalInfo()
    {
        notCheckedIndicator.SetActive(false);
    }


    #region JournalInfoUISubscriptions
    private void JournalInfoUI_OnJournalInfoCollected(object sender, EventArgs e)
    {
        CollectJournalInfo();
    }

    private void JournalInfoUI_OnJournalInfoChecked(object sender, EventArgs e)
    {
        CheckJournalInfo();
    }
    #endregion
}
