using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ordenador : MonoBehaviour
{
    #region Tareas aleatorias
    [SerializeField] public GameObject objectTareas;
    private TareasAleatorias tareasScript;
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
    [SerializeField] bool PlayerCerca;
    [SerializeField] bool TareaActiva;
    [SerializeField] bool TareaAcabada;
    private float save;
    [SerializeField] private float WinValue;
    [SerializeField] public Image spacebarsprite;
    [SerializeField] private float visibleTime = 0.2f;
    [SerializeField] private float WinThreshold = 60f; // valor para completar la tarea
    #endregion

    void Start()
    {
        tareasScript = objectTareas.GetComponent<TareasAleatorias>();
        TareaActiva = true;
        PlayerCerca = false;
        TareaAcabada = false;
        save = ValueBarStart;
        TaskBar.value = ValueBarStart;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !TareaAcabada)
        {
            PlayerCerca = true;
            GetComponent<MeshRenderer>().material = OutLine;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !TareaAcabada)
        {
            PlayerCerca = false;
            GetComponent<MeshRenderer>().material = Mat;
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
            TareaAcabada = true;
            Player.GetComponent<PlayerController>().playerOcupado = false;
            Player.GetComponent<OviedadZombie>().Zombiedad -= 0.2f;
            ValueBarStart = save;
            TaskBar.gameObject.SetActive(false);
            GetComponent<MeshRenderer>().material = Mat;

            // Completar en TareasAleatorias
            tareasScript.CompletarTarea(this.gameObject);

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
            Player.GetComponent<PlayerController>().playerOcupado = false;
            Player.GetComponent<OviedadZombie>().Zombiedad += 0.05f;
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
}
