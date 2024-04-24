using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalSender : MonoBehaviour
{
    [Header("Electrical Component")]
    [SerializeField] private Electrode electrode;

    [Header("Device Settings")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float shootTimer;

    private void OnEnable()
    {
        electrode.OnReceiveSignal += ShootSender;
    }

    private void OnDisable()
    {
        electrode.OnReceiveSignal -= ShootSender;
    }

    private void Update()
    {
        if(shootTimer > 0) shootTimer -= Time.deltaTime;
    }

    private void ShootSender()
    {
        if (shootTimer > 0) return;

        shootTimer = shootCooldown;

        float intensity = electrode.Power;

        if (intensity < Electrode.ACTIVATION_THRESHOLD) return;

        GameObject projectileGO = Instantiate(projectile, shootPosition.position, Quaternion.identity);

        SignalProjectile signalProjectile = projectileGO.GetComponent<SignalProjectile>();

        signalProjectile?.SetProjectile(gameObject, intensity);

        Rigidbody rbProjectile = projectileGO.GetComponent<Rigidbody>();

        rbProjectile?.AddForce(shootPosition.up.normalized * shootSpeed, ForceMode.Impulse);
    }
}
