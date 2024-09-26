using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowListener : MonoBehaviour
{
    [System.Serializable]
    public class CameraFollowSettings
    {
        public Transform targetTransform;
        [Range(0.5f, 4f)] public float stallTimeIn;
        [Range(0.5f, 4f)] public float moveInTime;
        [Range(0.5f, 10f)] public float stallTime;
        [Range(0.5f, 4f)] public float moveOutTime;
        [Range(0.5f, 4f)] public float stallTimeOut;
    }

    [Header("Shakes Settings")]
    [SerializeField] private CameraFollowSettings exampleSettings;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) 
        { 
            CameraFollowHandler.Instance.TransitionMoveCamera(exampleSettings.targetTransform, exampleSettings.stallTimeIn, exampleSettings.moveInTime, exampleSettings.stallTime, exampleSettings.moveOutTime, exampleSettings.stallTimeOut);
        }
    }
}
