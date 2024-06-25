using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PostProcessingUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected Button increaseIntensityButton;
    [SerializeField] protected Button decreaseIntensityButton;
    [SerializeField] protected Transform optionsBarsContainer;
    [SerializeField] protected Transform optionsBarSingleUIPrefab;

    protected PostProcessingManager postProcessingManager;

    protected const float INTENSITY_BUTTON_CHANGE = 0.1f;

    private void Awake()
    {
        InitializeButtonsListeners();
    }

    private void InitializeButtonsListeners()
    {
        increaseIntensityButton.onClick.AddListener(IncreaseIntensityByButton);
        decreaseIntensityButton.onClick.AddListener(DecreaseIntensityByButton);
    }

    private void Start()
    {
        InitializeUI();
    }

    protected abstract void SetPostProcessingManager();

    protected void InitializeUI()
    {
        SetPostProcessingManager();

        if (!postProcessingManager) return;

        UpdateVisual();
    }

    private void IncreaseIntensityByButton()
    {
        float currentIntensity = postProcessingManager.GetNormalizedIntensity();
        float desiredIntensity = currentIntensity + INTENSITY_BUTTON_CHANGE;

        if (desiredIntensity > postProcessingManager.GetMaxNormalizedIntensity()) return;

        desiredIntensity = GeneralMethods.RoundToNDecimalPlaces(desiredIntensity, 1);
        postProcessingManager.ChangeIntensity(desiredIntensity);
    }

    private void DecreaseIntensityByButton()
    {
        float currentIntensity = postProcessingManager.GetNormalizedIntensity();
        float desiredIntensity = currentIntensity - INTENSITY_BUTTON_CHANGE;

        if (desiredIntensity < postProcessingManager.GetMinNormalizedIntensity()) return;

        desiredIntensity = GeneralMethods.RoundToNDecimalPlaces(desiredIntensity, 1);
        postProcessingManager.ChangeIntensity(desiredIntensity);
    }

    protected void UpdateVisual()
    {
        if (!postProcessingManager) return;

        foreach (Transform child in optionsBarsContainer)
        {
            Destroy(child.gameObject);
        }

        int totalBars = Mathf.RoundToInt(postProcessingManager.GetMaxNormalizedIntensity() * 10f);
        int activeBars = Mathf.RoundToInt(postProcessingManager.GetNormalizedIntensity() * 10f);

        for (int i = 0; i < totalBars; i++)
        {
            Transform optionsBarTransform = Instantiate(optionsBarSingleUIPrefab, optionsBarsContainer);

            OptionsBarSingleUI optionsBarSingleUI = optionsBarTransform.GetComponent<OptionsBarSingleUI>();

            if (!optionsBarSingleUI)
            {
                Debug.LogWarning("The instantiated transform does not have a OptionsBarSingleUI component");
                continue;
            }

            if (activeBars > 0)
            {
                optionsBarSingleUI.EnableActiveIndicator();
                activeBars--;
            }
            else
            {
                optionsBarSingleUI.DisableActiveIndicator();
            }
        }
    }
}
