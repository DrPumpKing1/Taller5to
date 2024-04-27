using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Layers")]
    [SerializeField] private List<BaseUI> _UILayers;

    [Header("Components")]
    [SerializeField] private UIInput UIInput;

    public List<BaseUI> UILayers { get { return _UILayers; } }
    private bool CloseInput => UIInput.GetPauseDown();

    public static event EventHandler<OnUIToCloseInputEventArgs> OnUIToCloseInput;

    public class OnUIToCloseInputEventArgs : EventArgs
    {
        public BaseUI UIToClose;
    }


    private void Awake()
    {
        SetSingleton();
    }

    public void Update()
    {
        CheckUIToClose();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one UIManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    public void CheckUIToClose()
    {
        if (!CloseInput) return;
        if (!UIActive()) return;

        OnUIToCloseInput?.Invoke(this, new OnUIToCloseInputEventArgs { UIToClose = _UILayers[^1] });
    }

    public void AddToLayersList(BaseUI baseUI) => _UILayers.Add(baseUI);
    public void RemoveFromLayersList(BaseUI baseUI) => _UILayers.Remove(baseUI);
    public bool UIActive() => _UILayers.Count > 0;
}
