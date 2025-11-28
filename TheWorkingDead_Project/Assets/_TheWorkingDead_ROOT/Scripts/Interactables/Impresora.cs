using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Impresora : MonoBehaviour
{
    [Header("Tareas aleatorias")]
    [SerializeField] public GameObject objectTareas;
    private TareasAleatorias tareasScript;

    [Header("Variables generales")]
    [SerializeField] float ValueBarStart = 25;
    [SerializeField] float SumValue;
    [SerializeField] GameObject TaskBar;
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

    private Slider slider;

    void Start()
    {
        TareaActiva = false;
        PlayerCerca = false;
        save = ValueBarStart;
        slider = TaskBar.GetComponent<Slider>();
        slider.value = ValueBarStart;
        TareaAcabada = false;

        if (objectTareas != null)
            tareasScript = objectTareas.GetComponent<TareasAleatorias>();
        else
            Debug.LogError("objectTareas no asignado en Impresora: " + gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !TareaAcabada)
        {
            PlayerCerca = true;
            this.gameObject.GetComponent<MeshRenderer>().material = OutLine;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !TareaAcabada)
        {
            PlayerCerca = false;
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;
        }
    }

    void Update()
    {
        // Activación independiente
        TareaActiva = PlayerCerca && !TareaAcabada;

        slider.value = ValueBarStart;

        if (ValueBarStart >= 100 && !TareaAcabada)
        {
            TareaAcabada = true;
            Player.GetComponent<PlayerController>().playerOcupado = false;
            Player.GetComponent<OviedadZombie>().Zombiedad -= (20f / 100f);
            ValueBarStart = save;
            TaskBar.SetActive(false);
            this.gameObject.GetComponent<MeshRenderer>().material = Mat;
            tareasScript?.ganaTarea();
            StopAllCoroutines();
            this.enabled = false;
        }

        if (ValueBarStart <= 0 && !TareaAcabada)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad += (5f / 100f);
            ValueBarStart = save;
            TaskBar.SetActive(false);
            TareaActiva = false;
            Player.GetComponent<PlayerController>().playerOcupado = false;
            StopAllCoroutines();
        }

        // Marcar tarea en TareasAleatorias
        if (TareaAcabada && tareasScript != null)
        {
            for (int i = 0; i < tareasScript.OrdenTareas.Count; i++)
            {
                if (tareasScript.OrdenTareas[i] == this.gameObject)
                {
                    tareasScript.posTareaAcabada = i;
                    tareasScript.acabarTarea = true;
                    break;
                }
            }
        }
    }

    public void Interactuar()
    {
        if (TareaActiva && !TareaAcabada)
        {
            TaskBar.SetActive(true);
            StartCoroutine(WaitTaskBar(time));
            Player.GetComponent<PlayerController>().playerOcupado = true;
        }
    }

    public void TaskCode()
    {
        if (TareaActiva && !TareaAcabada)
            ValueBarStart += SumValue;

        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spacebarsprite.fillAmount = 100;
        yield return new WaitForSeconds(visibleTime);
        spacebarsprite.fillAmount = 0;
    }

    private IEnumerator WaitTaskBar(float duration)
    {
        while (ValueBarStart < 100 && ValueBarStart > 0)
        {
            yield return new WaitForSeconds(duration);
            ValueBarStart -= restValue;
        }
    }
}
