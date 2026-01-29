using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerReciclajeUI : MonoBehaviour
{
    [Header("Prefabs de objetos a spawnear")]
    public List<GameObject> todosLosObjetosUI;

    [Header("Área donde se spawnean los objetos")]
    public RectTransform areaSpawn;

    [Header("Cantidad por ronda")]
    public int cantidadPorRonda = 5;

    [Header("Espacio entre objetos")]
    public float spacing = 20f;

    void Start()
    {
        GenerarObjetos();
    }

    void GenerarObjetos()
    {
        if (todosLosObjetosUI == null || todosLosObjetosUI.Count == 0 || areaSpawn == null)
            return;

        List<GameObject> copia = new List<GameObject>(todosLosObjetosUI);
        int cantidad = Mathf.Min(cantidadPorRonda, copia.Count);

        List<RectTransform> objetosInstanciados = new List<RectTransform>();
        List<float> anchos = new List<float>();

        for (int i = 0; i < cantidad; i++)
        {
            int index = Random.Range(0, copia.Count);
            GameObject prefab = copia[index];
            copia.RemoveAt(index);

            GameObject obj = Instantiate(prefab, areaSpawn);
            RectTransform rt = obj.GetComponent<RectTransform>();
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rt);

            objetosInstanciados.Add(rt);
            anchos.Add(rt.rect.width);
        }

        float totalWidth = 0f;
        for (int i = 0; i < cantidad; i++)
        {
            totalWidth += anchos[i];
            if (i < cantidad - 1) totalWidth += spacing;
        }

        float startX = -totalWidth / 2f;
        float x = startX;

        for (int i = 0; i < cantidad; i++)
        {
            RectTransform rt = objetosInstanciados[i];
            float width = anchos[i];

            rt.anchoredPosition = new Vector2(x + width / 2f, 0f);
            x += width + spacing;
        }
    }
}
