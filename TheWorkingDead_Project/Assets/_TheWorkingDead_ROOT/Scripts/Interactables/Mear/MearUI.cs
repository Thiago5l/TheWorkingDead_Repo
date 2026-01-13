using UnityEngine;
using UnityEngine.InputSystem;

public class MearUI : MonoBehaviour
{
    [Header("Editor References")]
    [SerializeField] Transform camTransform; //ref  transform cámara

    [Header("Movement Parametres")]
    [SerializeField] float speed = 10f;
    [SerializeField] float speedcontainer = 10f;
    [SerializeField] float speedbase = 10f;
    [SerializeField] float rotSpeed = 15f;

    Rigidbody punteroRB;//ref a rigid boddy
    Vector2 moveImput;//almacén imput mov

    public GameObject objetoRetrete;
    [SerializeField] private TareaMear scriptMear;
    public GameObject meandoGreg;
    public GameObject puntero;
    public GameObject retreteUI;
    public float tiempoRecto;
    public float distanciaDesvio;
    public bool izquierdaDerecha;
    private Ray ray;

    private void Awake()
    {
        punteroRB = GetComponent<Rigidbody>();
        if (camTransform == null) camTransform = Camera.main.transform; //busca la cámara main si no tiene cam asignada
        punteroRB.freezeRotation = true; //congelar rotación de rigid body
        speedcontainer = speed;
        speedbase = speed;
        //sprintVFX.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scriptMear = objetoRetrete.GetComponent<TareaMear>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scriptMear.tareaEmpezada)
        {
            ray = new Ray(meandoGreg.transform.position, puntero.transform.position);
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
            Physics.Raycast(ray);

        }
    }
    void HandleMovement()
    {

        //almacenar dirección z + x de la cámara
        Vector3 cameraForward = Camera.main.transform.forward;//almacena el origen frontal de la cámara
        Vector3 cameraRight = Camera.main.transform.right;//almacena el origen lateral de la cámara

        //anular la orientación e y antes del cálculo de la orientación de la cámara aplicada a l movimiento
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize(); //el valor del float es máximo 1 por lo que no afecta a la multiplicación de velocidad
        cameraRight.Normalize(); //el valor del float es máximo 1 por lo que no afecta a la multiplicación de velocidad

        //se calcula y almacena la dirección x/z teniendo en cuenta la cámara por el imput
        Vector3 moveDireccion = (cameraForward * moveImput.y + cameraRight * moveImput.x).normalized;
        //una vez tenemos la dirección +el imput se lo aplicamos al motor de aceleración del rigidbody
        //todo esto sin afectar al eje y, porque eso se encargará el salto
        punteroRB.linearVelocity = new Vector3(moveDireccion.x * speed, punteroRB.linearVelocity.y, moveDireccion.z * speed);
    }
    public void OnMove(InputAction.CallbackContext context) //context bontón físico
    {
        if (!scriptMear.tareaEmpezada)
        {
            moveImput = context.ReadValue<Vector2>();
        }


    }
}
