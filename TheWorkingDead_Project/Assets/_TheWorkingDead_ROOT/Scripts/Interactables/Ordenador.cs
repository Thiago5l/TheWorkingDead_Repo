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
    [SerializeField] public Image spacebarsprite;
    [SerializeField] private float visibleTime = 0.2f;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TareaActiva = true;
        PlayerCerca = false;
        save = ValueBarStart;
        TaskBar.GetComponent<Slider>().value = ValueBarStart;
        TareaAcabada = false;

        tareasScript = objectTareas.GetComponent<TareasAleatorias>();

    }


    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && TareaAcabada == false)
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
        if (other.CompareTag("TaskPlayer") && TareaAcabada == false)
        {
            if (TareaActiva)
            {
                PlayerCerca = true;
                Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
                this.gameObject.GetComponent<MeshRenderer>().material = OutLine;
            }

        }

    }

    public void Interactuar()
    {
        if (PlayerCerca && !TareaAcabada)
        {

            if (TareaActiva)
            {
                Player.GetComponent<PlayerController>().playerOcupado = true;
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
            Player.GetComponent<OviedadZombie>().Zombiedad -= ((20)/100);
            ValueBarStart = save;
            TaskBar.gameObject.SetActive(false);
            Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;
            objectTareas.GetComponent<TareasAleatorias>().ganarTarea = true;
            TareaAcabada = true;
            StopAllCoroutines();
            WinValue = 0;
            TareaActiva = false;
            this.gameObject.GetComponent<Ordenador>().enabled = false;
        }

        if (TaskBar.value < 80 && TaskBar.value > 65)
        {
            StartCoroutine(WaitWinValue(0.5f));
        }
        else { WinValue = 0; }

        if (ValueBarStart <= 0)
        {
            Player.GetComponent<PlayerController>().playerOcupado = false;
            Player.GetComponent<OviedadZombie>().Zombiedad += ((5)/100);
            ValueBarStart = save;
            TareaActiva = false; 
            TaskBar.gameObject.SetActive(false);
            WinValue = 0;
            StopAllCoroutines();

        }


        if (ValueBarStart > 100) ValueBarStart = 100;
        if (TareaAcabada)
        {
            for (int i = 0; i < tareasScript.OrdenTareas.Count; i++)
            {
                if (tareasScript.OrdenTareas[i].CompareTag("Ordenador"))//cambiar que cambie la lista pa que me cambie un bool que si está en true me elimine la posición de la lista en la que está la tarea, actualizar la listade tareas de este código en el update para que sea siempre la mmisma de TareasAleatorias.cs
                {
                    Player.GetComponent<PlayerController>().playerOcupado = false;
                    objectTareas.GetComponent<TareasAleatorias>().posTareaAcabada = i;
                    objectTareas.GetComponent<TareasAleatorias>().acabarTarea = true;
                    return;
                }
            }

        }

    }


    

    public void TaskCode()
    {
        StartCoroutine(FlashRoutine());
        if (TareaActiva && !TareaAcabada)
        {
            ValueBarStart = ValueBarStart + SumValue;
            //StartCoroutine(FlashRoutine());
        }
    }
    private System.Collections.IEnumerator FlashRoutine()
    {
        spacebarsprite.fillAmount = 100;            // Mostrar
        yield return new WaitForSeconds(visibleTime);
        spacebarsprite.fillAmount = 0;           // Ocultar
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
