using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TareasAleatorias : MonoBehaviour
{
    [Header("Configuración de Tareas")]
    public GameObject[] PrefabsTareas;
    public int TareasPorNivel;

    [Header("Estado del nivel")]
    public int tareasHechas;
    public bool winLevel;

    [Header("UI y contenedor")]
    public GameObject boxTarea;
    public Transform tareaContiner;

    [Header("Control de tareas")]
    public List<GameObject> OrdenTareas = new List<GameObject>();
    public Dictionary<GameObject, GameObject> tareaToUI = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        DetectarNPCsComoTareas();
        MezclarLista(OrdenTareas);
        tareasHechas = 0;
        winLevel = false;
        GenerarListaTareas();
    }
    public void DetectarNPCsComoTareas()
    {
        // Encuentra todos los NPCs con el tag
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NpcConversation");

        // Convierte PrefabsTareas a lista para poder agregar NPCs
        List<GameObject> prefabsList = new List<GameObject>(PrefabsTareas);

        foreach (GameObject npc in npcs)
        {
            if (!prefabsList.Contains(npc))
            {
                prefabsList.Add(npc);
            }
        }

        // Vuelve a convertir a array
        PrefabsTareas = prefabsList.ToArray();

        Debug.Log($"Se detectaron {npcs.Length} NPCs por tag y se añadieron a PrefabsTareas. Total prefabs: {PrefabsTareas.Length}");
    }




    public void GenerarListaTareas()
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
        tareaToUI.Clear();

        for (int i = 0; i < TareasPorNivel; i++)
        {
            GameObject tarea = tareasDisponibles[i];
            OrdenTareas.Add(tarea);

            // Crear UI y asociarla directamente a la tarea
            GameObject box = Instantiate(boxTarea, tareaContiner);
            box.GetComponent<BoxTarea>().SetText(tarea.GetComponent<TareaNombre>().tareaNombre);
            tareaToUI[tarea] = box;
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
        if (OrdenTareas.Contains(tarea))
        {
            OrdenTareas.Remove(tarea);

            // Ocultar UI asociada a la tarea
            if (tareaToUI.ContainsKey(tarea))
            {
                tareaToUI[tarea].SetActive(false);
                tareaToUI.Remove(tarea);
            }

            tareasHechas++;
            Debug.Log($"Tarea completada: {tarea.name} | Restantes: {OrdenTareas.Count}");
        }
        else
        {
            Debug.LogWarning($"La tarea {tarea.name} no está en la lista de tareas pendientes.");
        }
    }

    void Update()
    {
        if (!winLevel && (tareasHechas >= TareasPorNivel || OrdenTareas.Count <= 0))
        {
            winLevel = true;
            SceneManager.LoadScene("SCN_CINE_WIN");
        }
    }
}
