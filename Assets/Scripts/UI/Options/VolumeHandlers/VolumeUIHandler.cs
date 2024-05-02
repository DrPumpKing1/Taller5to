using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class VolumeUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected Button increaseVolumeButton;
    [SerializeField] protected Button decreaseVolumeButton;
    [SerializeField] protected Transform volumeBarsContainer;
    [SerializeField] protected Transform volumeBarTemplate;

    protected VolumeManager volumeManager;

    protected const float VOLUME_BUTTON_CHANGE = 0.1f;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void InitializeButtonsListeners()
    {
        increaseVolumeButton.onClick.AddListener(IncreaseVolumeByButton);
        decreaseVolumeButton.onClick.AddListener(DecreaseVolumeByButton);
    }

    private void InitializeVariables()
    {
        volumeBarTemplate.gameObject.SetActive(false);
    }

    protected abstract void SetVolumeManager();

    private void IncreaseVolumeByButton()
    {
        float currentVolume = volumeManager.GetLinearVolume();
        float desiredVolume = currentVolume + VOLUME_BUTTON_CHANGE;

        if (desiredVolume > volumeManager.GetMaxVolume()) return;

        desiredVolume = GeneralMethods.RoundToNDecimalPlaces(desiredVolume, 1);
        volumeManager.ChangeVolume(desiredVolume);

        UpdateVisual();
    }

    private void DecreaseVolumeByButton()
    {
        float currentVolume = volumeManager.GetLinearVolume();
        float desiredVolume = currentVolume - VOLUME_BUTTON_CHANGE;

        if (desiredVolume < 0f) return;

        desiredVolume = GeneralMethods.RoundToNDecimalPlaces(desiredVolume, 1);
        volumeManager.ChangeVolume(desiredVolume);

        UpdateVisual();
    }

    protected void UpdateVisual()
    {
        foreach (Transform child in volumeBarsContainer)
        {
            if (child == volumeBarTemplate) continue;

            Destroy(child.gameObject);
        }

        int totalBars = Mathf.RoundToInt(volumeManager.GetMaxVolume() * 10f);
        int activeBars = Mathf.RoundToInt(volumeManager.GetLinearVolume() * 10f);

        for (int i = 0; i < totalBars; i++)
        {
            Transform volumeBarTransform = Instantiate(volumeBarTemplate, volumeBarsContainer);
            volumeBarTransform.gameObject.SetActive(true);

            VolumeBarSingleUI volumeBarSingleUI = volumeBarTransform.GetComponent<VolumeBarSingleUI>();

            if (!volumeBarSingleUI)
            {
                Debug.LogWarning("The instantiated transform does not have a VolumeBarSingleUI component");
                continue;
            }

            if (activeBars > 0)
            {
                volumeBarSingleUI.EnableActiveIndicator();
                activeBars--;
            }
            else
            {
                volumeBarSingleUI.DisableActiveIndicator();
            }
        }
    }
}

