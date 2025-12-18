using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnergeticasUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject energeticaPrefab;
    [SerializeField] Transform gridParent;

    private List<GameObject> icons = new List<GameObject>();

    public void SetEnergeticas(int amount)
    {
        // Crear iconos si faltan
        while (icons.Count < amount)
        {
            GameObject icon = Instantiate(energeticaPrefab, gridParent);
            icons.Add(icon);
        }

        // Activar / desactivar según cantidad
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].SetActive(i < amount);
        }
    }
}
