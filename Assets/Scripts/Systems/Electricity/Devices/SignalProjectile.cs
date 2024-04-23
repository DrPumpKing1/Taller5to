using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalProjectile : MonoBehaviour
{
    private const float LIFETIME = 3f;

    [SerializeField] private GameObject sender;
    [SerializeField] private float intensity;
    [SerializeField] private float lifespan;

    private void Start()
    {
        lifespan = LIFETIME;
    }

    private void Update()
    {
        if(lifespan >= 0) lifespan -= Time.deltaTime;
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetProjectile(GameObject sender, float intensity)
    {
        this.sender = sender;
        this.intensity = intensity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != sender)
        {
            Electrode component = collision.gameObject.GetComponent<Electrode>();

            if (component != null)
            {
                if(!component.Source) component.ReceiveSignal(intensity);
                else if(intensity > component.Power) component.ReceiveSignal(intensity);
            }

            ElectrodeCollider electroCollider = collision.gameObject.GetComponent<ElectrodeCollider>();

            if(electroCollider != null )
            {
                if(!electroCollider.Electrode.Source) electroCollider.Electrode.ReceiveSignal(intensity);
                else if(intensity < component.Power) component.ReceiveSignal(intensity);
            }

            Destroy(gameObject);
        }
    }
}
