using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("States")]
    [SerializeField] private State state;
    [SerializeField] private State previousState;

    public enum State { OnGameplay, OnUI, OnDialog }

    public State GameState { get { return state; } }

    private void OnEnable()
    {
        SymbolCrafingUI.OnAnySymbolCraftingUIOpen += SymbolCrafingUI_OnAnySymbolCraftingUIOpen;
        SymbolCrafingUI.OnAnySymbolCraftingUIClose += SymbolCrafingUI_OnAnySymbolCraftingUIClose;
    }

    private void OnDisable()
    {
        SymbolCrafingUI.OnAnySymbolCraftingUIOpen -= SymbolCrafingUI_OnAnySymbolCraftingUIOpen;
        SymbolCrafingUI.OnAnySymbolCraftingUIClose -= SymbolCrafingUI_OnAnySymbolCraftingUIClose;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        SetGameState(State.OnGameplay);
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("There is more than one GameManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void SetGameState(State state)
    {
        previousState = this.state;
        this.state = state;
    }

    #region SymbolCraftingUI Sumbcriptions
    private void SymbolCrafingUI_OnAnySymbolCraftingUIOpen(object sender, System.EventArgs e)
    {
        SetGameState(State.OnUI);
    }
    private void SymbolCrafingUI_OnAnySymbolCraftingUIClose(object sender, System.EventArgs e)
    {
        SetGameState(State.OnGameplay);
    }
    #endregion
}
