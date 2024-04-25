using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SymbolsAddedUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform symbolContainer;
    [SerializeField] private Transform symbolTemplate;

    [Header("Settings")]
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        DestroyAfterLifetime();
    }

    private void Start()
    {
        symbolTemplate.gameObject.SetActive(false);
    }

    private void DestroyAfterLifetime() => Destroy(transform.parent.gameObject, lifeTime);

    public void SetSymbolsUIs(List<DialectSymbolSO> dialectSymbolSOs)
    {
        foreach (DialectSymbolSO dialectSymbolSO in dialectSymbolSOs)
        {
            GameObject symbollUIGameObject = Instantiate(symbolTemplate.gameObject, symbolContainer);
            symbollUIGameObject.SetActive(true);

            SymbolAddedSingleUI symbolAddedSingleUI = symbollUIGameObject.GetComponent<SymbolAddedSingleUI>();

            if (!symbolAddedSingleUI)
            {
                Debug.LogWarning("There's not a SymbolAddedSingleUI attached to instantiated prefab");
                continue;
            }

            symbolAddedSingleUI.SetSymbolImage(dialectSymbolSO.symbolImage);
        }
    }
}
