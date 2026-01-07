using UnityEngine;

public class uiGiratoria : MonoBehaviour
{
    [SerializeField] public GameObject brazoPrefab;
    [SerializeField] private BrazoCaido brazoScript;

    //[SerializeField] public bool miniGameStarted;
    [SerializeField] public float rotacionTotal;
    private float anguloAnterior;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        brazoPrefab = GameObject.FindGameObjectWithTag("BrazoCaido");
        brazoScript = brazoPrefab.GetComponent<BrazoCaido>();
        rotacionTotal = 0;
        anguloAnterior = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (brazoScript.miniGameStarted)
        {
            
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector2 direction = mousePosition - transform.position;
            transform.up = direction;

            float anguloActual = transform.eulerAngles.z;
            float delta = Mathf.DeltaAngle(anguloAnterior, anguloActual);

            // Suma progresiva (360 grados = +1)
            brazoScript.rotacionTotal += delta / 360f;

            anguloAnterior = anguloActual;


            //Vector3 mousePosition = Input.mousePosition;
            //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition); // Distancia desde la cámara

            //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            //// Lógica que se ejecuta cuando el juego ha comenzado
            //transform.up = direction;
        }
    }
}
