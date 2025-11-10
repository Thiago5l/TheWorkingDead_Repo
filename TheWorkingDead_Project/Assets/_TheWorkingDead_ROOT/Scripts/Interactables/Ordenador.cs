using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Ordenador : MonoBehaviour
{


    #region Tareas aleatorias
    [SerializeField] public GameObject objectTareas;
    private TareasAleatorias tareasScript;
    #endregion


    #region General Variables

    [SerializeField] float ValueBarStart = 25;
    [SerializeField] float SumValue;
    [SerializeField] Slider TaskBar;
    [SerializeField] GameObject Player;
    [SerializeField] float restValue;
    [SerializeField] float time;
    [SerializeField] Material Mat;
    [SerializeField] Material OutLine;
    [SerializeField] bool PlayerCerca;
    [SerializeField] bool TareaActiva;
    [SerializeField] bool TareaAcabada;
    private float save;
    [SerializeField] private float WinValue;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TareaActiva = true;
        PlayerCerca = false;
        save = ValueBarStart;
        TareaAcabada = false;

        tareasScript = objectTareas.GetComponent<TareasAleatorias>();

}


    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && TareaAcabada == false)
        {
            if (TareaActiva)
            {
                PlayerCerca = true;
                Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
                this.gameObject.GetComponent<MeshRenderer>().material = OutLine;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && TareaAcabada == false)
        {
            PlayerCerca = false;
            Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;

        }

    }

    public void Interactuar()
    {
        if (PlayerCerca && !TareaAcabada)
        {

            if (TareaActiva)
            {
                TaskBar.gameObject.SetActive(true);
                StartCoroutine(WaitTaskBar(time));
            }
        }
    }

    private void Update()
    {




        if (tareasScript.OrdenTareas[0] == this.gameObject)
        {
            TareaActiva = true;
        }
        else { TareaActiva = false; }


        TaskBar.value = ValueBarStart;

        if (WinValue >= 60)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad -= 20;
            ValueBarStart = save;
            TaskBar.gameObject.SetActive(false);
            Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;
            TareaAcabada = true;
            StopAllCoroutines();
            WinValue = 0;
            TareaActiva = false;
            this.gameObject.GetComponent<Ordenador>().enabled = false;
        }

        if (TaskBar.value < 85 && TaskBar.value > 75)
        {
            StartCoroutine(WaitWinValue(0.5f));
        }
        else { WinValue = 0; }

        if (ValueBarStart <= 0)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad += 5;
            ValueBarStart = save;
            TareaActiva = false; 
            TaskBar.gameObject.SetActive(false);
            StopAllCoroutines();
            WinValue = 0;

        }
        if (TareaAcabada)
        {
            for (int i = 0; i < tareasScript.OrdenTareas.Count; i++)
            {
                if (tareasScript.OrdenTareas[i].CompareTag("Ordenador"))//cambiar que cambie la lista pa que me cambie un bool que si está en true me elimine la posición de la lista en la que está la tarea, actualizar la listade tareas de este código en el update para que sea siempre la mmisma de TareasAleatorias.cs
                {
                    objectTareas.GetComponent<TareasAleatorias>().posTareaAcabada = i;
                    objectTareas.GetComponent<TareasAleatorias>().acabarTarea = true;
                    return;
                }
            }

        }

    }


    

    public void TaskCode()
    {
        if (TareaActiva && !TareaAcabada)
        {
            ValueBarStart = ValueBarStart + SumValue;
        }
    }

    IEnumerator WaitWinValue(float duration)
    {
        yield return new WaitForSeconds(duration);
        WinValue += 1;

    }


    IEnumerator WaitTaskBar(float duration)
    {
        while (ValueBarStart < 100 && ValueBarStart > 0)
        {
            yield return new WaitForSeconds(duration);
            ValueBarStart = ValueBarStart - restValue;
        }

    }
}
