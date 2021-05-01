using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelector : MonoBehaviour
{
    [SerializeField] private Material freeSquareMaterial;
    [SerializeField] private Material opponetSquareMaterial;
    [SerializeField] private GameObject selectorPrefab;
    private List<GameObject> instantiatedSelectors = new List<GameObject>();

    public void ShowSelection(Dictionary<Vector3, bool> squareData) {
        ClearSelection();
        foreach (var item in squareData)
        {
            GameObject selector = Instantiate(selectorPrefab, item.Key, Quaternion.identity);
            instantiatedSelectors.Add(selector);
            foreach (var matSetter in selector.GetComponentsInChildren<MaterialSetter>())
            {
                matSetter.SetSingleMaterial(item.Value ? freeSquareMaterial : opponetSquareMaterial);
            }
        }
    }

    public void ClearSelection() {
        for (int i = 0; i < instantiatedSelectors.Count; i++)
            Destroy(instantiatedSelectors[i]);
    }
}
