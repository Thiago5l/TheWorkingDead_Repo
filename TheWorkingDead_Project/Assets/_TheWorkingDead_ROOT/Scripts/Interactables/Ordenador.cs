using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ordenador : MonoBehaviour
{
    #region Tareas aleatorias
    [SerializeField] public GameObject objectTareas;
    private TareasAleatorias taskmanager;
    #endregion

    #region Variables generales
    [SerializeField] float ValueBarStart = 25;
    [SerializeField] float SumValue;
    [SerializeField] Slider TaskBar;
    [SerializeField] GameObject Player;
    [SerializeField] float restValue;
    [SerializeField] float time;
    [SerializeField] Material Mat;
    [SerializeField] Material OutLine;
    [SerializeField] public bool PlayerCerca;
    [SerializeField] bool TareaActiva;
    [SerializeField] bool TareaAcabada;
    private float save;
    [SerializeField] private float WinValue;
    [SerializeField] public Image spacebarsprite;
    [SerializeField] private float visibleTime = 0.2f;
    [SerializeField] private float WinThreshold = 60f; // valor para completar la tarea
    [SerializeField] public GameObject CanvasInteractableKey;
    [SerializeField] private FadeCanvas taskFeedbackCanvas;
    public bool tieneTarea;
    #endregion
    public bool EstaEnListaDeTareas()
    {
        if (taskmanager == null)
        {
            Debug.LogWarning("TaskManager no asignado.");
            return false;
        }

        // Devuelve true si este GameObject está en la lista de tareas pendientes
        return taskmanager.OrdenTareas.Contains(this.gameObject);
    }
    void Start()
    {
        taskmanager = objectTareas.GetComponent<TareasAleatorias>();
        TareaActiva = true;
        PlayerCerca = false;
        TareaAcabada = false;
        save = ValueBarStart;
        TaskBar.value = ValueBarStart;
        CanvasInteractableKey.SetActive(false);
        tieneTarea = EstaEnListaDeTareas();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !TareaAcabada)
        {
            PlayerCerca = true;
            GetComponent<MeshRenderer>().material = OutLine;
            CanvasInteractableKey.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !TareaAcabada)
        {
            PlayerCerca = false;
            GetComponent<MeshRenderer>().material = Mat;
            CanvasInteractableKey.SetActive(false);
        }
    }

    void Update()
    {
        // Actualizar si la tarea sigue activa
        TareaActiva = PlayerCerca && !TareaAcabada;

        // Actualizar barra
        TaskBar.value = ValueBarStart;

        // Completar tarea
        if (WinValue >= WinThreshold && !TareaAcabada)
        {
            taskFeedbackCanvas.PlayWin();
            CanvasInteractableKey.SetActive(false);
            TareaAcabada = true;
            Player.GetComponent<PlayerController>().playerOcupado = false;

            ValueBarStart = save;
            TaskBar.gameObject.SetActive(false);
            GetComponent<MeshRenderer>().material = Mat;

            // Completar en TareasAleatorias
            taskmanager.CompletarTarea(this.gameObject);

            StopAllCoroutines();
            WinValue = 0;
            TareaActiva = false;
            this.enabled = false;
        }

        // Incremento de WinValue cuando la barra está en rango
        if (ValueBarStart > 65 && ValueBarStart < 80)
        {
            StartCoroutine(IncrementWinValue(0.5f));
        }
        else
        {
            WinValue = 0;
        }

        // Fallo de tarea
        if (ValueBarStart <= 0)
        {
            taskFeedbackCanvas.PlayLose();
            Player.GetComponent<PlayerController>().playerOcupado = false;
            ValueBarStart = save;
            TareaActiva = false;
            TaskBar.gameObject.SetActive(false);
            WinValue = 0;
            StopAllCoroutines();
        }

        // Limitar barra
        if (ValueBarStart > 100) ValueBarStart = 100;
    }

    public void Interactuar()
    {
        if (TareaActiva && !TareaAcabada)
        {
            CanvasInteractableKey.SetActive(false);
            Player.GetComponent<PlayerController>().playerOcupado = true;
            TaskBar.gameObject.SetActive(true);
            StartCoroutine(DecrementTaskBar(time));
        }
    }

    public void TaskCode()
    {
        if (TareaActiva && !TareaAcabada)
        {
            ValueBarStart += SumValue;
            if (ValueBarStart > 100) ValueBarStart = 100;
            StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {
        spacebarsprite.fillAmount = 1f;
        yield return new WaitForSeconds(visibleTime);
        spacebarsprite.fillAmount = 0f;
    }

    private IEnumerator IncrementWinValue(float duration)
    {
        yield return new WaitForSeconds(duration);
        WinValue += 1;
    }

    private IEnumerator DecrementTaskBar(float duration)
    {
        while (ValueBarStart > 0 && ValueBarStart < 100)
        {
            yield return new WaitForSeconds(duration);
            ValueBarStart -= restValue;
        }
    }
    public void cerrar()
    {
        Player.GetComponent<PlayerController>().playerOcupado = false;
        ValueBarStart = save;
        TareaActiva = false;
        TaskBar.gameObject.SetActive(false);
        WinValue = 0;
        StopAllCoroutines();
        CanvasInteractableKey.SetActive(true);
    }
}
