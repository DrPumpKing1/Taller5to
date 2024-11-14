using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientRelicModelVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AncientRelic ancientRelic;
    [SerializeField] private Transform visual;
    [SerializeField] private Collider _collider;

    private void OnEnable()
    {
        ancientRelic.OnCollection += AncientRelic_OnCollection;
    }

    private void OnDisable()
    {
        ancientRelic.OnCollection -= AncientRelic_OnCollection;
    }

    private void DisableVisual() => visual.gameObject.SetActive(false);
    private void DisableCollider() => _collider.enabled = false;

    private void AncientRelic_OnCollection(object sender, System.EventArgs e)
    {
        DisableVisual();
        DisableCollider();
    }
}
