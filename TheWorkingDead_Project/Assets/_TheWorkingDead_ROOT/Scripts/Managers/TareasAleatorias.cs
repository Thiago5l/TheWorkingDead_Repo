using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TareasAleatorias : MonoBehaviour
{
    [Header("Configuración de Tareas")]
    [SerializeField] public GameObject[] PrefabsTareas;
    [SerializeField] public int TareasPorNivel;

    [Header("Estado del nivel")]
    [SerializeField] public int tareasHechas;
    [SerializeField] public bool winLevel;

    [Header("UI y contenedor")]
    [SerializeField] public GameObject boxTarea;
    [SerializeField] public Transform tareaContiner;

    [Header("Control de tareas")]
    [SerializeField] public List<GameObject> OrdenTareas = new List<GameObject>();

    void Start()
    {
        tareasHechas = 0;
        winLevel = false;
        GeneradorListaTareas();
    }

    public void GeneradorListaTareas()
    {
        if (PrefabsTareas == null || PrefabsTareas.Length == 0)
        {
            Debug.LogError("PrefabsTareas está vacío!");
            return;
        }

        if (TareasPorNivel > PrefabsTareas.Length)
            TareasPorNivel = PrefabsTareas.Length;

        List<GameObject> tareasDisponibles = new List<GameObject>(PrefabsTareas);
        MezclarLista(tareasDisponibles);

        OrdenTareas.Clear();

        for (int i = 0; i < TareasPorNivel; i++)
        {
            GameObject tarea = tareasDisponibles[i];
            OrdenTareas.Add(tarea);

            // Crear UI
            GameObject box = Instantiate(boxTarea, tareaContiner);
            box.GetComponent<BoxTarea>().SetText(tarea.GetComponent<TareaNombre>().tareaNombre);
        }

        Debug.Log($"Se generaron {OrdenTareas.Count} tareas aleatorias");
    }

    private void MezclarLista(List<GameObject> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = lista[i];
            lista[i] = lista[j];
            lista[j] = temp;
        }
    }

    // Método público para completar cualquier tarea
    public void CompletarTarea(GameObject tarea)
    {
        int index = OrdenTareas.IndexOf(tarea);
        if (index >= 0)
        {
            OrdenTareas.RemoveAt(index);

            // Ocultar UI correspondiente
            if (index < tareaContiner.childCount)
                tareaContiner.GetChild(index).gameObject.SetActive(false);

            tareasHechas++;
            Debug.Log($"Tarea completada: {tarea.name} | Restantes: {OrdenTareas.Count}");
        }
    }

    void Update()
    {
        if (tareasHechas >= TareasPorNivel || OrdenTareas.Count <= 0)
            winLevel = true;

        if (winLevel)
            SceneManager.LoadScene("SCN_CINE_WIN");
    }
}
