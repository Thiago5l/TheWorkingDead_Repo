using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.WebRequestMethods;

public class TareasAleatorias : MonoBehaviour
{





    //[Header("Configuración de Tareas")]
    //[SerializeField] public GameObject[] PrefabsTareas;

    //[SerializeField] public int TareasPorNivel;

    //[Header("Resultado")]
    //[Tooltip("La lista final de tareas seleccionadas para el nivel, en orden aleatorio.")]
    //[SerializeField] public GameObject[] OrdenTareas;

    //// Se llama automáticamente cuando el juego inicia.
    //void Start()
    //{
    //    GenerarListaDeTareas();
    //}

    //public void GenerarListaDeTareas()
    //{
    //    if (PrefabsTareas == null || PrefabsTareas.Length == 0)
    //    {
    //        Debug.LogError("El array 'PrefabsTareas' está vacío o no ha sido asignado en el Inspector.");
    //        return;
    //    }

    //    if (TareasPorNivel > PrefabsTareas.Length)
    //    {
    //        Debug.LogWarning("Se Necesitan más tareas ('TareasPorNivel') de las disponibles en 'PrefabsTareas'. Se usarán todas las tareas disponibles.");
    //        TareasPorNivel = PrefabsTareas.Length;
    //    }

    //    List<GameObject> tareasDisponibles = new List<GameObject>(PrefabsTareas);

    //    MezclarLista(tareasDisponibles);

    //    OrdenTareas = new GameObject[TareasPorNivel];
    //    for (int i = 0; i < TareasPorNivel; i++)
    //    {
    //        OrdenTareas[i] = tareasDisponibles[i];
    //    }

    //    Debug.Log($"Se generó una lista con {OrdenTareas.Length} tareas aleatorias.");
    //}


    //private void MezclarLista<T>(List<T> lista)
    //{

    //    int tamaño = lista.Count;
    //    int RandomVal;
    //    T Temp;

    //    for (int i = 0; i < tamaño; i++)
    //    {
    //        RandomVal = Random.Range(0, tamaño);
    //        Temp = lista[RandomVal];
    //        lista[RandomVal] = lista[i];
    //        lista[i] = Temp;

    //    }
    //}










    [Header("Configuración de Tareas")]
    [SerializeField] public GameObject[] PrefabsTareas;
    [SerializeField] public int tareasHechas;
    [SerializeField] public bool winLevel;
    [SerializeField] public GameObject boxTarea;
    [SerializeField] public Transform tareaContiner;



    [SerializeField] public int posTareaAcabada;
    [SerializeField] public bool acabarTarea;
    [SerializeField] public bool cambiarDeTarea;



    [SerializeField] public List<GameObject> OrdenTareas;
    [SerializeField] public int TotalTareas;
    [SerializeField] public int TareasPorNivel;


    [SerializeField] public GameObject player;
    [SerializeField] public bool ganaTarea;


    void Start()
    {
        ganaTarea = false;
        cambiarDeTarea = false;
        acabarTarea = false;
        winLevel = false;
        GeneradorListaTareas();
        tareasHechas = 0;
    }

    public void GeneradorListaTareas()
    {
        // Validar que tengamos suficientes tareas
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

        //OrdenTareas = new List<GameObject>;

        for (int i = 0; i < TareasPorNivel; i++)
        {
            GameObject tempSvd = tareasDisponibles[i];
            OrdenTareas.Add(tempSvd);
            SetNameTasks(tempSvd.GetComponent<TareaNombre>().tareaNombre);
        }

        Debug.Log($"Se generaron {OrdenTareas.Count} tareas aleatorias");
    }

    private void MezclarLista(List<GameObject> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            GameObject temp = lista[i];
            lista[i] = lista[j];
            lista[j] = temp;

        }
    }

    void Update()
    {
        if (tareasHechas == TareasPorNivel)
        {
            winLevel = true;
        }

        if (acabarTarea == true)
        {
            Debug.Log($"Eliminando tarea en posición {posTareaAcabada}");
            OrdenTareas.RemoveAt(posTareaAcabada);
            Debug.Log($"Lista actual: {OrdenTareas.Count} tareas restantes");
            acabarTarea = false;
        }


        if (OrdenTareas.Count <= 0)
        {
            winLevel= true;
        }
        if (winLevel == true)
        {
            SceneManager.LoadScene("SCN_WIN_CINE");
        }

        if(ganaTarea == true)
        {
            Debug.Log("llamada función eliminar tarea ");
            DeleteTaskList();
        }


    }



    private void DeleteTaskList()
    {

        Debug.Log("función eliminar tarea activada");
        //Destroy(tareaContiner.GetChild(0).gameObject);
        tareaContiner.GetChild(0).gameObject.SetActive(false); 
        ganaTarea = false;
    }
    


    private void SetNameTasks(string nameTxt)
    {
        GameObject box = Instantiate(boxTarea, tareaContiner);
        box.GetComponent<BoxTarea>().SetText(nameTxt);

    }

}














//[SerializeField] public GameObject[] PrefabsTareas;
//[SerializeField] public GameObject[] OrdenTareas;
//[SerializeField] public int TotalTareas;
//[SerializeField] public int TareasPorNivel;
//private List<GameObject> tareasSeleccionadas;


//// Start is called once before the first execution of Update after the MonoBehaviour is created
//void Start()
//{
//    GeneradorListaTareas(PrefabsTareas, OrdenTareas);
//    IgualarListasPorPosicion(PrefabsTareas, OrdenTareas);
//}

//public void GeneradorListaTareas<GameObject>(GameObject[] listaA, GameObject[] listaB)
//{
//    tareasSeleccionadas.Clear();

//    int tamañoA= listaA.Length;
//    int RandomVal;
//    GameObject Temp; 

//    if (listaA != null && tamañoA > 0)
//    {
//        for (int i = 0; i < tamañoA; i++)
//        {
//            RandomVal = Random.Range(0, tamañoA);
//            Temp = listaA[RandomVal];
//            listaA[RandomVal] = listaA[i];
//            listaA[i] = Temp;

//        }           
//    }



//}
//public void IgualarListasPorPosicion<GameObject>(GameObject[] listaA, GameObject[] listaB)
//{
//    GameObject SVal;

//    for (int i = 0; i < TareasPorNivel; i++)
//    {
//        SVal = listaA[i];
//        Debug.Log(SVal.ToString());
//    }
//}
