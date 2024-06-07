using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlayerParent : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG)) return;

        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG)) return;

        collision.transform.SetParent(null);
    }
}
