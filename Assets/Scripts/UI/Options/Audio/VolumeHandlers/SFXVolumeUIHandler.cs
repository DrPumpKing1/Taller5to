using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXVolumeUIHandler : VolumeUIHandler
{
    private void OnEnable()
    {
        SFXVolumeManager.OnSFXVolumeManagerInitialized += SFXVolumeManager_OnSFXVolumeManagerInitialized;
    }
    private void OnDisable()
    {
        SFXVolumeManager.OnSFXVolumeManagerInitialized -= SFXVolumeManager_OnSFXVolumeManagerInitialized;
    }

    protected override void SetVolumeManager() => volumeManager = SFXVolumeManager.Instance;
    private void SFXVolumeManager_OnSFXVolumeManagerInitialized(object sender, System.EventArgs e)
    {
        SetVolumeManager();
        UpdateVisual();
    }
}
