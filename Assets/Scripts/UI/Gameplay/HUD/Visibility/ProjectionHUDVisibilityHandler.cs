using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionHUDVisibilityHandler : HUDVisibilityHandler
{
    public static ProjectionHUDVisibilityHandler Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        SetSingleton();
    }
    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one HUDVisibilityHandler instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }
}
