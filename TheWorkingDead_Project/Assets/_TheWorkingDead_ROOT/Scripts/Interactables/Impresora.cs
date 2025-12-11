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
    [SerializeField] public bool PlayerCerca;
    [SerializeField] bool TareaAcabada;

    [SerializeField] public Image spacebarsprite;
    [SerializeField] private float visibleTime = 0.2f;

    [SerializeField] public GameObject CanvasInteractableKey;

    private Slider slider;
    private float save;

    void Start()
    {
        tareasScript = objectTareas.GetComponent<TareasAleatorias>();
        save = ValueBarStart;
        slider = TaskBar.GetComponent<Slider>();
        slider.value = ValueBarStart;
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
        if (other.CompareTag("TaskPlayer"))
        {
            PlayerCerca = false;
            GetComponent<MeshRenderer>().material = Mat;
            CanvasInteractableKey.SetActive(false);
        }
    }

    void Update()
    {
        slider.value = ValueBarStart;
        bool TareaActiva = PlayerCerca && !TareaAcabada;

        if (ValueBarStart >= 100 && !TareaAcabada)
        {
            CanvasInteractableKey.SetActive(false);
            TareaAcabada = true;
            Player.GetComponent<PlayerController>().playerOcupado = false;
            Player.GetComponent<OviedadZombie>().Zombiedad -= (20f / 100f);
            ValueBarStart = save;
            TaskBar.SetActive(false);
            GetComponent<MeshRenderer>().material = Mat;

            // Marcar tarea completada en TareasAleatorias
            tareasScript.CompletarTarea(this.gameObject);

            StopAllCoroutines();
            this.enabled = false;
        }

        if (ValueBarStart <= 0 && !TareaAcabada)
        {
            Player.GetComponent<OviedadZombie>().Zombiedad += (5f / 100f);
            ValueBarStart = save;
            TaskBar.SetActive(false);
            Player.GetComponent<PlayerController>().playerOcupado = false;
            StopAllCoroutines();
        }
    }

    public void Interactuar()
    {
        if (PlayerCerca && !TareaAcabada)
        {
            CanvasInteractableKey.SetActive(false);
            TaskBar.SetActive(true);
            StartCoroutine(WaitTaskBar(time));
            Player.GetComponent<PlayerController>().playerOcupado = true;
        }
    }

    public void TaskCode()
    {
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

    public void cerrar()
    {
        CanvasInteractableKey.SetActive(true);
        ValueBarStart = save;
        TaskBar.SetActive(false);
        Player.GetComponent<PlayerController>().playerOcupado = false;
        StopAllCoroutines();
    }
}
