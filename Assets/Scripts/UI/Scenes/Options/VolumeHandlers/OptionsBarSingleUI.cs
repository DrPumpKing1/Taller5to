using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsBarSingleUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform activeIndicator;

    public void EnableActiveIndicator() => activeIndicator.gameObject.SetActive(true);
    public void DisableActiveIndicator() => activeIndicator.gameObject.SetActive(false);
}
