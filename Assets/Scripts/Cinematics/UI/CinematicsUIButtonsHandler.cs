using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicsUIButtonsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button skipCinematicButton;

    private void Awake()
    {
        IntializeButtonsListeners();
    }

    private void IntializeButtonsListeners()
    {
        skipCinematicButton.onClick.AddListener(SkipCinematicScene);
    }

    private void SkipCinematicScene() => CinematicsManager.Instance.SkipCinematic();
}
