using UnityEngine;
using UnityEngine.UI;

public class BrazoCaido : MonoBehaviour
{
    public bool miniGameStarted;
    public float rotacionTotal;
    public float rotacionMax;
    public GameObject canvasTarea;
    public Slider progresoSlider;
    public bool playerCerca;
    public GameObject player;
    public GameObject CanvasInteractableKey;
    public float sumValueZombiedad;
    public GameObject pfBrazo;

    private bool resultadoEnviado = false;

    public GameObject brazoL;
    public FadeCanvas feedBackCanva;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playerCerca = true;
            CanvasInteractableKey.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playerCerca = false;
            CanvasInteractableKey.SetActive(false);
        }
    }
    private void Awake()
    {
        progresoSlider.value = 0;
        canvasTarea.SetActive(false);
        rotacionTotal = 0;
        miniGameStarted = false;

        progresoSlider.maxValue = rotacionMax;

        player = GameObject.FindGameObjectWithTag("Player");
        if (brazoL == null)
            brazoL = GameObject.FindGameObjectWithTag("BrazoL");
        pfBrazo.SetActive(false);
        if (feedBackCanva == null)
            feedBackCanva = FindAnyObjectByType<FadeCanvas>();
    }

    public void interactuar()
    {
        if (playerCerca && !miniGameStarted)
        {
            CanvasInteractableKey.SetActive(false);
            player.GetComponent<PlayerController>().playerOcupado = true;

            miniGameStarted = true;
            resultadoEnviado = false;

            rotacionTotal = 0;
            progresoSlider.value = 0;

            canvasTarea.SetActive(true);
        }
    }

    void Update()
    {
        if (!miniGameStarted) return;

        rotacionTotal = canvasTarea.GetComponentInChildren<uiGiratoria>().rotacionTotal;
        if (rotacionTotal < 0) rotacionTotal *= -1;

        progresoSlider.value = rotacionTotal;

        if (rotacionTotal >= rotacionMax && !resultadoEnviado)
        {
            resultadoEnviado = true;

            miniGameStarted = false;
            canvasTarea.SetActive(false);
            progresoSlider.value = progresoSlider.maxValue;

            player.GetComponent<PlayerController>().playerOcupado = false;

            brazoL.SetActive(true);

            feedBackCanva.GetComponent<FadeCanvas>().brazoYaCaido = false;

            rotacionTotal = 0;
            progresoSlider.value = 0;
            canvasTarea.GetComponentInChildren<uiGiratoria>().rotacionTotal = 0;

            pfBrazo.SetActive(false);
        }
    }

    public void cerrar()
    {
        CanvasInteractableKey.SetActive(true);
        miniGameStarted = false;
        resultadoEnviado = false;

        player.GetComponent<PlayerController>().playerOcupado = false;
        canvasTarea.SetActive(false);
    }
}
