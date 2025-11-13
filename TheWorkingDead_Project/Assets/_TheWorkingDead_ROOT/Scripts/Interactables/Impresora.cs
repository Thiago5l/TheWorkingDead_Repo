using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
public class Impresora : MonoBehaviour
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
    [SerializeField] public Image spacebarsprite;
    [SerializeField] private float visibleTime = 0.2f;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TareaActiva = false;
        PlayerCerca = false;
        save = ValueBarStart;
        TareaAcabada = false;

        tareasScript = objectTareas.GetComponent<TareasAleatorias>();
    }
   

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && TareaAcabada == false )
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
        if(PlayerCerca && !TareaAcabada)
        {
            if (TareaActiva)
            {
                TaskBar.gameObject.SetActive(true);
                StartCoroutine(WaitTaskBar(time));
            }


            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && TareaAcabada == false)
        {
            PlayerCerca = false;
            Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;

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

        if (ValueBarStart >= 100)
        {

            Player.GetComponent<OviedadZombie>().Zombiedad -= 20;
            ValueBarStart = save;
            TaskBar.gameObject.SetActive(false);
            Destroy(this.gameObject.GetComponent<MeshRenderer>().material);
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;
            Debug.Log("acabar tarea en true");
            objectTareas.GetComponent<TareasAleatorias>().ganaTarea = true;
            TareaAcabada = true;
            StopAllCoroutines();
            TareaActiva = false;
            this.gameObject.GetComponent<Impresora>().enabled = false;


        }
        if (ValueBarStart <= 0)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad += 5;
            ValueBarStart = save;
            TaskBar.gameObject.SetActive(false);
            TareaActiva = false;
            StopAllCoroutines();


        }

        if (TareaAcabada)
        {
            for (int i = 0; i < tareasScript.OrdenTareas.Count; i++)
            {
                if (tareasScript.OrdenTareas[i].CompareTag("Impresora"))
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
        StartCoroutine(FlashRoutine());

    }
    private System.Collections.IEnumerator FlashRoutine()
    {
        spacebarsprite.fillAmount = 100;            // Mostrar
        yield return new WaitForSeconds(visibleTime);
       spacebarsprite.fillAmount = 0;           // Ocultar
    }


    IEnumerator WaitTaskBar(float duration)
    {
        while (ValueBarStart < 100 && ValueBarStart>0)
        {
            yield return new WaitForSeconds(duration);
            ValueBarStart = ValueBarStart - restValue;
        }

    }



}
   

