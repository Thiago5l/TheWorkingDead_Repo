using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TareasAleatorias : MonoBehaviour
{
    [Header("Configuracion de Tareas")]
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

    [Header("Flags de tarea")]
    [SerializeField] public bool ganarTarea;
    [SerializeField] public int posTareaAcabada;
    [SerializeField] public bool acabarTarea;

    void Start()
    {
        ganarTarea = false;
        acabarTarea = false;
        winLevel = false;
        tareasHechas = 0;

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
        {
            Debug.LogWarning("TareasPorNivel es mayor que PrefabsTareas disponibles. Ajustando...");
            TareasPorNivel = PrefabsTareas.Length;
        }

        List<GameObject> tareasDisponibles = new List<GameObject>(PrefabsTareas);
        MezclarLista(tareasDisponibles);

        OrdenTareas.Clear();

        for (int i = 0; i < TareasPorNivel; i++)
        {
            GameObject temp = tareasDisponibles[i];
            OrdenTareas.Add(temp);

            // Crear UI
            GameObject box = Instantiate(boxTarea, tareaContiner);
            box.GetComponent<BoxTarea>().SetText(temp.GetComponent<TareaNombre>().tareaNombre);
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

    void Update()
    {
        // Revisar si se completo el nivel
        if (tareasHechas >= TareasPorNivel || OrdenTareas.Count <= 0)
        {
            winLevel = true;
        }

        if (winLevel)
        {
            SceneManager.LoadScene("SCN_CINE_WIN");
            return;
        }

        // Revisar si se ganó alguna tarea
        if (ganarTarea)
        {
            tareasHechas++;
            ganarTarea = false;
        }

        // Revisar flag de acabar tarea desde otra clase
        if (acabarTarea)
        {
            if (posTareaAcabada >= 0 && posTareaAcabada < OrdenTareas.Count)
            {
                // Ocultar o destruir UI
                if (posTareaAcabada < tareaContiner.childCount)
                    tareaContiner.GetChild(posTareaAcabada).gameObject.SetActive(false);

                OrdenTareas.RemoveAt(posTareaAcabada);
            }
            acabarTarea = false;
        }
    }

    // Método público para marcar una tarea como completada
    public void ganaTarea()
    {
        ganarTarea = true;
    }
}
