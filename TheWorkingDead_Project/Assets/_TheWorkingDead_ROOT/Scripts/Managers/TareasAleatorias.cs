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
    [Header("Tareas obligatorias (siempre entran)")]
    public List<GameObject> tareasObligatorias = new List<GameObject>();

    [Header("Configuración NPCs")]
    public int maxNPCsEnTareas = 3; // número máximo de NPCs que se agregan a PrefabsTareas
    public GameObject Uwe;

    [Header("Win")]
    public GameObject wincanvas;
    void Start()
    {
        wincanvas.SetActive(false);
        DetectarNPCsComoTareas();
        MezclarLista(OrdenTareas);
        tareasHechas = 0;
        winLevel = false;
        GenerarListaTareas();
    }
    public void DetectarNPCsComoTareas()
    {
        // Encuentra todos los NPCs activos en la escena con el tag correcto
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NpcConversation");

        if (npcs.Length == 0)
        {
            Debug.LogWarning("No se detectaron NPCs con el tag 'NpcConversation'");
            return;
        }

        // Convierte PrefabsTareas a lista para agregar NPCs
        List<GameObject> prefabsList = new List<GameObject>(PrefabsTareas);

        // Mezclar NPCs para seleccionar aleatoriamente
        List<GameObject> npcsList = new List<GameObject>(npcs);
        for (int i = npcsList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = npcsList[i];
            npcsList[i] = npcsList[j];
            npcsList[j] = temp;
        }

        // Agregar solo hasta maxNPCsEnTareas NPCs
        int count = Mathf.Min(maxNPCsEnTareas, npcsList.Count);
        for (int i = 0; i < count; i++)
        {
            GameObject npc = npcsList[i];
            if (!prefabsList.Contains(npc))
            {
                prefabsList.Add(npc);
            }
        }

        PrefabsTareas = prefabsList.ToArray();
        Debug.Log($"Se detectaron {npcs.Length} NPCs, se agregaron {count} a PrefabsTareas. Total prefabs: {PrefabsTareas.Length}");
    }

    public void GenerarListaTareas()
    {
        if (PrefabsTareas == null || PrefabsTareas.Length == 0)
        {
            Debug.LogError("PrefabsTareas esta vacio");
            return;
        }

        List<GameObject> tareasDisponibles = new List<GameObject>(PrefabsTareas);

        OrdenTareas.Clear();
        tareaToUI.Clear();

        // Agregar tareas obligatorias
        foreach (GameObject tarea in tareasObligatorias)
        {
            if (tarea != null && !OrdenTareas.Contains(tarea))
            {
                OrdenTareas.Add(tarea);
                tareasDisponibles.Remove(tarea);
            }
        }

        // Uwe opcional
        bool incluirUwe = (Uwe != null && Random.value < 0.5f);

        if (incluirUwe && !OrdenTareas.Contains(Uwe))
        {
            OrdenTareas.Add(Uwe);
            tareasDisponibles.Remove(Uwe);
        }

        // Tareas aleatorias restantes
        MezclarLista(tareasDisponibles);

        int restantes = TareasPorNivel - OrdenTareas.Count;

        for (int i = 0; i < restantes && i < tareasDisponibles.Count; i++)
        {
            OrdenTareas.Add(tareasDisponibles[i]);
        }

        // Crear UI
        foreach (GameObject tarea in OrdenTareas)
        {
            if (tarea == null) continue;

            GameObject box = Instantiate(boxTarea, tareaContiner);

            TareaNombre tareaComp = tarea.GetComponent<TareaNombre>();
            string nombreTarea = tareaComp != null ? tareaComp.tareaNombre : tarea.name;

            box.GetComponent<BoxTarea>().SetText(nombreTarea);
            tareaToUI[tarea] = box;
        }

        Debug.Log("Tareas generadas: " + OrdenTareas.Count);
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
            wincanvas.SetActive(true);
        }
    }
}
