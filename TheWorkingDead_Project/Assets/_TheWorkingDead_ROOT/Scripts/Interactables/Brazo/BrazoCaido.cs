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

    private float anguloAnterior;
    private float rotacionAcumulada;

    public GameObject brazoL;

    public GameObject feedBackCanva;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            Debug.Log("Entra");
            playerCerca = true;
            CanvasInteractableKey.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            Debug.Log("Sale");
            playerCerca = false;
            CanvasInteractableKey.SetActive(false);
        }
    }


    void OnEnable()
    {
        progresoSlider.value = 0;
        canvasTarea.SetActive(false);
        rotacionTotal = 0;
        miniGameStarted = false;
        anguloAnterior = transform.eulerAngles.z;
        //if(progresoSlider != null && progresoSlider.maxValue <= rotacionMax )
        //{
        //}
        progresoSlider.maxValue = rotacionMax;

        //feedBackCanva = GameObject.FindGameObjectWithTag("Feedback");
        //player = GameObject.FindGameObjectWithTag("Player");
        //brazoL = GameObject.FindGameObjectWithTag("BrazoCaido");   
        brazoL.gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    private void Awake()
    {
        
    }

    public void interactuar()
    {
        if (playerCerca && !miniGameStarted)
        {
            CanvasInteractableKey.SetActive(false);
            player.GetComponent<PlayerController>().playerOcupado = true;
            miniGameStarted = true;
            canvasTarea.SetActive(true);
            
        }


        // Aquí va la lógica para interactuar con el Brazo Caído    
    }
    // Update is called once per frame
    void Update()
    {
        
        rotacionTotal = canvasTarea.GetComponentInChildren<uiGiratoria>().rotacionTotal;
        if (rotacionTotal < 0) rotacionTotal = rotacionTotal * -1;
        progresoSlider.value = rotacionTotal;
        if (rotacionTotal >= rotacionMax)
        {
            miniGameStarted = false;
            canvasTarea.SetActive(false);
            progresoSlider.value = progresoSlider.maxValue;

            // Aquí va la lógica para completar el minijuego
            player.GetComponent<PlayerController>().playerOcupado = false;
            brazoL.gameObject.SetActive(true);

            feedBackCanva.GetComponent<FadeCanvas>().brazoYaCaido = false;
            feedBackCanva.GetComponent<FadeCanvas>().PlayWin();

            Destroy(pfBrazo);
        }


        //if (miniGameStarted)
        //{

        //    Vector3 mousePosition = Input.mousePosition;
        //    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //    Vector2 direction = mousePosition - transform.position;
        //    transform.up = direction;

        //    float anguloActual = transform.eulerAngles.z;
        //    float delta = Mathf.DeltaAngle(anguloAnterior, anguloActual);

        //    // Suma progresiva (360 grados = +1)
        //    rotacionTotal += delta / 360f;

        //    anguloAnterior = anguloActual;

        //    Debug.Log("Rotación acumulada: " + rotacionTotal);


        //    //Vector3 mousePosition = Input.mousePosition;
        //    //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); // Distancia desde la cámara

        //    //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        //    //// Lógica que se ejecuta cuando el juego ha comenzado
        //    //transform.up = direction;
        //}
    }
    public void cerrar()
    {
        CanvasInteractableKey.SetActive(true);
        miniGameStarted = false;
        player.GetComponent<PlayerController>().playerOcupado = false;
        canvasTarea.SetActive(false);
    }

}
