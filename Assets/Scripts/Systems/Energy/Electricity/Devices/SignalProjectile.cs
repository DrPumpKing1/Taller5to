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

            component?.ReceiveSignal(intensity);

            ElectrodeCollider electroCollider = collision.gameObject.GetComponent<ElectrodeCollider>();

            if(electroCollider != null )
            {
                electroCollider.Electrode.ReceiveSignal(intensity);
            }

            Destroy(gameObject);
        }
    }
}
