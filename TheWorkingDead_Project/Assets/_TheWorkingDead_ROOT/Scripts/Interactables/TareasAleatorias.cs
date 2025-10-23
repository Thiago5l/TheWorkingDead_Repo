using UnityEngine;
using System.Collections.Generic;

public class TareasAleatorias : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> PrefabsTareas = new List<GameObject>();
    [SerializeField] public List<int> OrdenTareas = new List<int>();
    [SerializeField] public int TotalTareas;
    [SerializeField] public int TareasPorNivel;
    private List<GameObject> tareasSeleccionadas = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GeneradorListaTareas();
    }

    void GeneradorListaTareas()
    {
        tareasSeleccionadas.Clear();

        if (PrefabsTareas != null && PrefabsTareas.Count > 0)
        {
                
        }
       /* NTareas.Clear();
        for (int i = 0; i < TotalTareas; i++) 
        { 
            NTareas.Add(i);
        }

        for (int i = NTareas.Count -1; i >0; i--)
        {
            OrdenTareas[i] = Random.Range(1, 3);

            for (int j = 0; j < 2; j++)
            {
                if (OrdenTareas[i] == NTareas[j])
                {
                    //NTareas.Remove(j);
                }
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
