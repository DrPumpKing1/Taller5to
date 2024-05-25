using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InstructionsUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI instructionsText;

    [Header("Settings")]
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        DestroyRootAfterLifeTime();
    }

    public void SetInstructionsText(string instruction) => instructionsText.text = instruction;

    private void DestroyRootAfterLifeTime() => Destroy(transform.root.gameObject, lifeTime);
}
