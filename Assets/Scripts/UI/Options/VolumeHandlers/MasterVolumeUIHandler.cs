using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterVolumeUIHandler : VolumeUIHandler
{
    protected override void SetVolumeManager() => volumeManager = MasterVolumeManager.Instance;

    private void OnEnable()
    {
        MasterVolumeManager.OnMasterVolumeManagerInitialized += MasterVolumeManager_OnMasterVolumeManagerInitialized;
    }
    private void OnDisable()
    {
        MasterVolumeManager.OnMasterVolumeManagerInitialized -= MasterVolumeManager_OnMasterVolumeManagerInitialized;
    }

    private void MasterVolumeManager_OnMasterVolumeManagerInitialized(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }
}
