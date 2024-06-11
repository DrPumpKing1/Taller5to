using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlayerParent : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private Transform player;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG)) return;

        collision.transform.SetParent(transform);
        player = collision.transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG)) return;

        collision.transform.SetParent(null);
        player = null;
    }

    private void OnDestroy()
    {
        if (player)
        {
            player.SetParent(null);
        }
    }
}
