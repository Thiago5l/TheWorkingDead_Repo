using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnerReciclajeUI : MonoBehaviour
{
    public List<GameObject> todosLosObjetosUI;
    public RectTransform areaSpawn;

    public int cantidadPorRonda = 5;
    public float padding = 30f;
    public float spacing = 20f;

    void Start()
    {
        GenerarObjetos();
    }

    void GenerarObjetos()
    {
        if (todosLosObjetosUI.Count == 0 || areaSpawn == null) return;

        List<GameObject> copia = new List<GameObject>(todosLosObjetosUI);

        float x = -areaSpawn.rect.width / 2 + padding;
        float y = areaSpawn.rect.height / 2 - padding;

        float filaAlturaMax = 0f;

        for (int i = 0; i < cantidadPorRonda; i++)
        {
            if (copia.Count == 0) break;

            int prefabIndex = Random.Range(0, copia.Count);
            GameObject prefab = copia[prefabIndex];
            copia.RemoveAt(prefabIndex);

            GameObject obj = Instantiate(prefab, areaSpawn);
            RectTransform rt = obj.GetComponent<RectTransform>();

            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);

            float w = rt.rect.width;
            float h = rt.rect.height;

            // Salto de línea si no cabe
            if (x + w > areaSpawn.rect.width / 2 - padding)
            {
                x = -areaSpawn.rect.width / 2 + padding;
                y -= filaAlturaMax + spacing;
                filaAlturaMax = 0f;
            }

            rt.anchoredPosition = new Vector2(x + w / 2, y - h / 2);

            x += w + spacing;
            filaAlturaMax = Mathf.Max(filaAlturaMax, h);
        }
    }
}