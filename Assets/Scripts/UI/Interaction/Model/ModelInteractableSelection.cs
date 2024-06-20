using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelInteractableSelection : MonoBehaviour
{
    [Header("Interactable Components")]
    [SerializeField] private Component interactableComponent;

    [Header("Model List")]
    [SerializeField] private List<Transform> models;

    private IInteractable interactable;

    private void OnEnable()
    {
        interactable.OnObjectSelected += Interactable_OnObjectSelected;
        interactable.OnObjectDeselected += Interactable_OnObjectDeselected;
    }

    private void OnDisable()
    {
        interactable.OnObjectSelected -= Interactable_OnObjectSelected;
        interactable.OnObjectDeselected -= Interactable_OnObjectDeselected;
    }

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        HideModels();
    }

    private void InitializeComponents()
    {
        interactable = interactableComponent.GetComponent<IInteractable>();
        if (interactable == null) Debug.LogError("The interactable component does not implement IInteractable");
    }

    public void ShowModels()
    {
        foreach(Transform model in models)
        {
            model.gameObject.SetActive(true);
        }
    }

    public void HideModels()
    {
        foreach (Transform model in models)
        {
            model.gameObject.SetActive(false);
        }
    }


    #region IInteractable Event Subscriptions
    private void Interactable_OnObjectSelected(object sender, System.EventArgs e)
    {
        ShowModels();
    }

    private void Interactable_OnObjectDeselected(object sender, System.EventArgs e)
    {
        HideModels();
    }
    #endregion
}
