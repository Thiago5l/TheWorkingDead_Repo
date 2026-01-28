using System.Collections.Generic;
using UnityEngine;

public class SpawnerReciclajeUI : MonoBehaviour
{
    public List<GameObject> todosLosObjetosUI;
    public RectTransform areaSpawn;

    public int cantidadPorRonda = 5;
    public float spacing = 20f; // Espacio entre objetos

    void Start()
    {
        GenerarObjetos();
    }

    void GenerarObjetos()
    {
        if (todosLosObjetosUI.Count == 0 || areaSpawn == null) return;

        // Hacer copia para no repetir objetos
        List<GameObject> copia = new List<GameObject>(todosLosObjetosUI);
        int cantidad = Mathf.Min(cantidadPorRonda, copia.Count);

        // Calcular ancho total para centrar
        float totalWidth = 0f;
        List<RectTransform> tempRTs = new List<RectTransform>();
        for (int i = 0; i < cantidad; i++)
        {
            GameObject prefab = copia[i];
            RectTransform rtPrefab = prefab.GetComponent<RectTransform>();
            if (rtPrefab == null) continue;

            totalWidth += rtPrefab.rect.width;
            if (i < cantidad - 1) totalWidth += spacing;
            tempRTs.Add(rtPrefab);
        }

        float startX = -totalWidth / 2f;
        float x = startX;

        for (int i = 0; i < cantidad; i++)
        {
            // Elegir prefab aleatorio y eliminarlo para no repetir
            int index = Random.Range(0, copia.Count);
            GameObject prefab = copia[index];
            copia.RemoveAt(index);

            GameObject obj = Instantiate(prefab, areaSpawn);
            RectTransform rt = obj.GetComponent<RectTransform>();
            tempRTs.Add(rt);

            // Posición horizontal en línea recta, centrada
            rt.anchoredPosition = new Vector2(x + rt.rect.width / 2f, 0f);
            x += rt.rect.width + spacing;
        }
    }
}
