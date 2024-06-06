using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolumeUIHandler : VolumeUIHandler
{
    private void OnEnable()
    {
        MusicVolumeManager.OnMusicVolumeManagerInitialized += MusicVolumeManager_OnMusicVolumeManagerInitialized;
    }
    private void OnDisable()
    {
        MusicVolumeManager.OnMusicVolumeManagerInitialized += MusicVolumeManager_OnMusicVolumeManagerInitialized;
    }

    protected override void SetVolumeManager() => volumeManager = MusicVolumeManager.Instance;
    private void MusicVolumeManager_OnMusicVolumeManagerInitialized(object sender, System.EventArgs e)
    {
        SetVolumeManager();
        UpdateVisual();
    }
}
