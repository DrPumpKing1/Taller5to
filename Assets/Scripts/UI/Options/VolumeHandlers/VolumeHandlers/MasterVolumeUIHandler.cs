using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeUIHandler : VolumeUIHandler
{
    private void OnEnable()
    {
        MasterVolumeManager.OnMasterVolumeManagerInitialized += MasterVolumeManager_OnMasterVolumeManagerInitialized;
    }
    private void OnDisable()
    {
        MasterVolumeManager.OnMasterVolumeManagerInitialized -= MasterVolumeManager_OnMasterVolumeManagerInitialized;
    }

    protected override void SetVolumeManager() => volumeManager = MasterVolumeManager.Instance;
    private void MasterVolumeManager_OnMasterVolumeManagerInitialized(object sender, System.EventArgs e)
    {
        InitializeUI();
    }
}
