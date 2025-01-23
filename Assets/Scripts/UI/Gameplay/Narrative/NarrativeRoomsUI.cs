using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeRoomsUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image feedbackImage; 

    [Header("Settings")]
    [SerializeField,Range(0f,10f)] private float fadeInTime;
    [SerializeField, Range(0f, 10f)] private float stallTime;
    [SerializeField, Range(0f, 10f)] private float fadeOutTime;
    [Space]
    [SerializeField, Range(0f, 10f)] private float stallPower;
    [SerializeField, Range(0f, 10f)] private float stallIntensity;
    [Space]
    [SerializeField, Range(1f, 10f)] private float lerpFactor; 
    [Header("States")]
    [SerializeField] private State state;

    private enum State {Hidden, FadingIn, Stall, FadingOut}

    //More Power -> Less Visible
    //More Intensity ->  More Visible

    private const float HIDDEN_POWER = 10f;
    private const float HIDDEN_INTENSITY = 0f;

    private Material feedbackMaterial;

    private float currentPower;
    private float currentIntensity;

    #region Material Properties

    private const string POWER_PROPERTY = "_Power";
    private const string INTENSITY_PROPERTY = "_Intensity";

    #endregion

    private void OnEnable()
    {
        NarrativeRoomsManager.OnNarrativeRoomVisited += NarrativeRoomsManager_OnNarrativeRoomVisited;
    }
    private void OnDisable()
    {
        NarrativeRoomsManager.OnNarrativeRoomVisited -= NarrativeRoomsManager_OnNarrativeRoomVisited;
    }

    private void Awake()
    {
        AssignFields();
    }

    private void Start()
    {
        InitializeVariables();
        SetState(State.Hidden);
    }

    private void AssignFields()
    {
        feedbackMaterial = feedbackImage.material;
    }

    private void InitializeVariables()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
        currentPower = HIDDEN_POWER;
        currentIntensity = HIDDEN_INTENSITY;
    }

    private void SetState(State state) => this.state = state;

    private void ShowNarrativeRoomVisitedFeedback()
    {
        StopAllCoroutines();
        StartCoroutine(ShowNarrativeRoomVisitedFeedbackCoroutine());
    }

    private  IEnumerator ShowNarrativeRoomVisitedFeedbackCoroutine()
    {
        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 1f);

        SetState(State.FadingIn);
        yield return StartCoroutine(FadeInCoroutine());
        SetState(State.Stall);
        yield return new WaitForSeconds(stallTime);
        SetState(State.FadingOut);
        yield return StartCoroutine(FadeOutCoroutine());
        SetState(State.Hidden);

        GeneralUIMethods.SetCanvasGroupAlpha(canvasGroup, 0f);
    }

    private IEnumerator FadeInCoroutine()
    {
        float timer = 0f;

        float intensityChangePerSecond = (stallIntensity - HIDDEN_INTENSITY) / fadeInTime;
        float powerChangePerSecond = (stallPower - HIDDEN_POWER) / fadeInTime;

        SetFeedbackMaterialIntensity(HIDDEN_INTENSITY);
        SetFeedbackMaterialPower(HIDDEN_POWER);

        float currentIntensity = HIDDEN_INTENSITY;
        float currentPower = HIDDEN_POWER;

        while (timer <= fadeInTime)
        {
            //currentIntensity += intensityChangePerSecond * Time.deltaTime;
            //currentPower += powerChangePerSecond * Time.deltaTime;

            currentIntensity = Mathf.Lerp(currentIntensity, stallIntensity, lerpFactor / fadeInTime * Time.deltaTime);
            currentPower = Mathf.Lerp(currentPower, stallPower, lerpFactor / fadeInTime * Time.deltaTime);

            SetFeedbackMaterialIntensity(currentIntensity);
            SetFeedbackMaterialPower(currentPower);

            timer += Time.deltaTime;
            yield return null;
        }

        SetFeedbackMaterialIntensity(stallIntensity);
        SetFeedbackMaterialPower(stallPower);
    }


    private IEnumerator FadeOutCoroutine()
    {
        float timer = 0f;

        float intensityChangePerSecond = (HIDDEN_INTENSITY - stallIntensity) / fadeOutTime;
        float powerChangePerSecond = (HIDDEN_POWER - stallPower) / fadeOutTime;

        SetFeedbackMaterialIntensity(stallIntensity);
        SetFeedbackMaterialPower(stallPower);

        float currentIntensity = stallIntensity;
        float currentPower = stallPower;

        while (timer <= fadeOutTime)
        {
            currentIntensity += intensityChangePerSecond * Time.deltaTime;
            currentPower += powerChangePerSecond * Time.deltaTime;

            SetFeedbackMaterialIntensity(currentIntensity);
            SetFeedbackMaterialPower(currentPower);

            timer += Time.deltaTime;
            yield return null;
        }

        SetFeedbackMaterialIntensity(HIDDEN_INTENSITY);
        SetFeedbackMaterialPower(HIDDEN_POWER);
    }

    private void SetFeedbackMaterialPower(float power)
    {
        if (!feedbackMaterial.HasFloat(POWER_PROPERTY)) return;

        feedbackMaterial.SetFloat(POWER_PROPERTY, power);
    }

    private void SetFeedbackMaterialIntensity(float intensity)
    {
        if (!feedbackMaterial.HasFloat(INTENSITY_PROPERTY)) return;

        feedbackMaterial.SetFloat(INTENSITY_PROPERTY, intensity);
    }

    #region NarrativeRoomsManager Subscriptions
    private void NarrativeRoomsManager_OnNarrativeRoomVisited(object sender, NarrativeRoomsManager.OnNarrativeRoomEventArgs e)
    {
        ShowNarrativeRoomVisitedFeedback();
    }
    #endregion
}
