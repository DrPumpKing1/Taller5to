using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalSFXController : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}
