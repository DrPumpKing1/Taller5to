using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawSpot : MonoBehaviour
{
    [SerializeField] private Vector2 position;
    public Vector2 Position { get { return position; } }
    public void SetSpotData(Vector2 position)
    {
        this.position = position;
    }
}
