using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalTrap : MonoBehaviour
{
    public Electrode electrode;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(electrode.Power >= Electrode.ACTIVATION_THRESHOLD)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
