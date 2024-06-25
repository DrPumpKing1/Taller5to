using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloomIntensityUIHandler : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] protected Button increaseIntensityButton;
    [SerializeField] protected Button decreaseIntensityButton;
    [SerializeField] protected Transform optionsBarsContainer;
    [SerializeField] protected Transform optionsBarSingleUIPrefab;

    protected const float INTENSITY_BUTTON_CHANGE = 0.1f;

    private void OnEnable()
    {
        BloomIntensityManager.OnBloomIntensityManagerInitialized += BloomIntensityManager_OnBloomIntensityManagerInitialized;
        BloomIntensityManager.OnBloomIntensityChanged += BloomIntensityManager_OnBloomIntensityChanged;
    }

    private void OnDisable()
    {
        BloomIntensityManager.OnBloomIntensityManagerInitialized -= BloomIntensityManager_OnBloomIntensityManagerInitialized;
        BloomIntensityManager.OnBloomIntensityChanged -= BloomIntensityManager_OnBloomIntensityChanged;
    }

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

    protected void InitializeUI()
    {
        UpdateVisual();
    }

    private void IncreaseIntensityByButton()
    {
        float currentIntensity = BloomIntensityManager.Instance.GetNormalizedIntensity();
        float desiredIntensity = currentIntensity + INTENSITY_BUTTON_CHANGE;

        if (desiredIntensity > BloomIntensityManager.Instance.GetMaxNormalizedIntensity()) return;

        desiredIntensity = GeneralMethods.RoundToNDecimalPlaces(desiredIntensity, 1);
        BloomIntensityManager.Instance.ChangeIntensity(desiredIntensity);
    }

    private void DecreaseIntensityByButton()
    {
        float currentIntensity = BloomIntensityManager.Instance.GetNormalizedIntensity();
        float desiredIntensity = currentIntensity - INTENSITY_BUTTON_CHANGE;

        if (desiredIntensity < 0f) return;

        desiredIntensity = GeneralMethods.RoundToNDecimalPlaces(desiredIntensity, 1);
        BloomIntensityManager.Instance.ChangeIntensity(desiredIntensity);
    }

    protected void UpdateVisual()
    {
        foreach (Transform child in optionsBarsContainer)
        {
            Destroy(child.gameObject);
        }

        int totalBars = Mathf.RoundToInt(BloomIntensityManager.Instance.GetMaxNormalizedIntensity() * 10f);
        int activeBars = Mathf.RoundToInt(BloomIntensityManager.Instance.GetNormalizedIntensity() * 10f);

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

    private void BloomIntensityManager_OnBloomIntensityManagerInitialized(object sender, System.EventArgs e)
    {
        InitializeUI();
    }

    private void BloomIntensityManager_OnBloomIntensityChanged(object sender, BloomIntensityManager.OnIntensityChangedEventArgs e)
    {
        UpdateVisual();
    }
}
