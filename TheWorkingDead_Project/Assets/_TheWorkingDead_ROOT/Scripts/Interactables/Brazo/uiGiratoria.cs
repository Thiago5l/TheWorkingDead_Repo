using UnityEngine;
using UnityEngine.UI;

public class uiGiratoria : MonoBehaviour
{
    public GameObject brazoPrefab;
    [SerializeField] private BrazoCaido brazoScript;
    [SerializeField] private Slider progresoSlider;
    [SerializeField] private Vector3 scalaOriginal;

    //[SerializeField] public bool miniGameStarted;
    public float rotacionTotal;
    private float anguloAnterior;

    private float porcentajeTamaño;
    [SerializeField] private float scaleMax = 1.5f; // tamaño máximo
    //private Vector3 escalaOriginal;

    private Quaternion targetRotation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //brazoPrefab = GameObject.FindGameObjectWithTag("BrazoCaido");
        //brazoScript = brazoPrefab.GetComponent<BrazoCaido>();
        //rotacionTotal = 0;
        anguloAnterior = 0;
        scalaOriginal = this.transform.localScale;
        targetRotation = transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!brazoScript.miniGameStarted) return;

        Vector2 mousePos = Input.mousePosition;
        Vector2 uiPos = transform.position;

        Vector2 direction = mousePos - uiPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // -90 porque el sprite mira hacia arriba
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        float anguloActual = transform.eulerAngles.z;

        float delta = Mathf.DeltaAngle(anguloAnterior, anguloActual);

        // 360 grados = 1 unidad
        rotacionTotal += delta / 360f;
        // if (rotacionTotal < 0) rotacionTotal = rotacionTotal * -1;
        anguloAnterior = anguloActual;



        float valorBarra = brazoScript.progresoSlider.value;
        float maxValueBarra = brazoScript.progresoSlider.maxValue;
        porcentajeTamaño = (valorBarra * maxValueBarra) / 100;
        if (porcentajeTamaño < 1)
        {
            porcentajeTamaño += 1;
        }
        this.transform.localScale = scalaOriginal * porcentajeTamaño;

        //float scaleAmount = Mathf.Abs(anguloActual)* scaleFactor;
        //scaleAmount = Mathf.Min(scaleAmount, maxScale - initialScale.x);
        //Vector3 newScale = initialScale + Vector3.one * scaleAmount;
        //this.transform.localScale = newScale;
        //this.gameObject.transform.localScale *= porcentajeTam;
        //if (this.gameObject.transform.localScale.x < scalaOriginal.x) this.gameObject.transform.localScale = scalaOriginal;

        //if (brazoScript.miniGameStarted)
        //{
        //    //Vector3 mouseWorldPos = Input.mousePosition;
        //    //mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    //Vector2 lookAtDirection = new Vector2(mouseWorldPos.x - this.transform.position.x, mouseWorldPos.y - this.transform.position.y); // mouseWorldPos - this.transform.position;
        //    //this.transform.up = lookAtDirection;



        //    //Vector3 mousePosition = Input.mousePosition;
        //    //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //    //Vector2 direction = mousePosition - transform.position;
        //    //Debug.Log("dirección = " + direction + "transform.u valor = " +transform.up);
        //    //transform.up = direction;

        //    //float anguloActual = transform.eulerAngles.z;
        //    //Debug.Log("ángulo actual = " + anguloActual + " ángulo anterior = " + anguloAnterior);
        //    //float delta = Mathf.DeltaAngle(anguloAnterior, anguloActual);

        //    //// Suma progresiva (360 grados = +1)
        //    //brazoScript.rotacionTotal += delta / 360f;

        //    //anguloAnterior = anguloActual;


        //    //Vector3 mousePosition = Input.mousePosition;
        //    //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); // Distancia desde la cámara

        //    //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        //    //// Lógica que se ejecuta cuando el juego ha comenzado
        //    //transform.up = direction;
        //}

        //// Smoothly rotate towards the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
