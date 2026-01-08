using UnityEngine;

public class uiGiratoria : MonoBehaviour
{
    [SerializeField] public GameObject brazoPrefab;
    [SerializeField] private BrazoCaido brazoScript;

    //[SerializeField] public bool miniGameStarted;
     public float rotacionTotal;
    private float anguloAnterior;


    private Quaternion targetRotation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //brazoPrefab = GameObject.FindGameObjectWithTag("BrazoCaido");
        //brazoScript = brazoPrefab.GetComponent<BrazoCaido>();
        //rotacionTotal = 0;
        anguloAnterior = 0;

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
