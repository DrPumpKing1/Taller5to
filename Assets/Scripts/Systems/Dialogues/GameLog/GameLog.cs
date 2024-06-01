using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLog : MonoBehaviour
{
    public event Action OnLogAdd;

    public static GameLog Instance { get; private set; }

    [Header("Log")] 
    public List<GameplayAction> gameLog;

    [Serializable]
    public struct GameplayAction
    {
        public float time;
        public string log;
    }
     
    private void Awake()
    {
        SetSingleton();      
        InitializeLog();
    }

    private void Start()
    {
        Log("GameFlow/Start");
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one GameLog instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeLog()
    {
        gameLog = new List<GameplayAction>();
    }

    public static void Log(string log)
    {
        Instance.gameLog.Add(new GameplayAction
        {
            time = Time.time,
            log = log
        });
        
        Instance.OnLogAdd?.Invoke();
    }
}
