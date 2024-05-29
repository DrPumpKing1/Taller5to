using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLog : MonoBehaviour
{
    [Serializable]
    public struct GameplayAction
    {
        public float time;
        public string log;
    }
    
    public static GameLog Instance;

    [Header("Log")] 
    public List<GameplayAction> gameLog;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There are more than one Game Log in the Scene");
            return;
        }

        Instance = this;
        
        InitializeLog();
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
    }
}
