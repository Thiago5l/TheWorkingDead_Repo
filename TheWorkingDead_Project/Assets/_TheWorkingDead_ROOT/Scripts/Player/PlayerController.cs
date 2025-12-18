using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{


    #region General Variables
    [Header("Editor References")]
    [SerializeField] Transform camTransform; //ref  transform cámara
    
    [SerializeField] float TaskDetectorRad;
    [SerializeField] LayerMask interactablesLayer;

    [Header("Movement Parametres")]
    [SerializeField] float speed = 10f;
    [SerializeField] float speedcontainer = 10f;
    [SerializeField] float speedbase = 10f;
    [SerializeField] float rotSpeed = 15f;


    [Header("jump parametres")]
    [SerializeField] Transform GroundCheck; //ref posición desde la que se detecta el suelo
    [SerializeField] float groundCheckRadius = 0.2f; //rando detección
    [SerializeField] LayerMask groundLayer; //capa detección suelo

    [Header("task parametres")]
    public bool playerOcupado;
    public bool  playerCerca;
    public GameObject TaskCollider;
    public LayerMask taskLayer;

    [Header("sprint parametres")]
    [SerializeField]public float energeticas = 1;
    [SerializeField]public float sprintspeed = 10f;
    [SerializeField]public float sprinttime = 3f;
    public bool isSprinting;
    //variables referencia propias o internas


    Rigidbody PlayerRB;//ref a rigid boddy
    Vector2 moveImput;//almacén imput mov
    bool isGrounded;//determina si estás tocando el suelo
    bool TaskDetector;
    #endregion

    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody>();
        if(camTransform == null) camTransform = Camera.main.transform; //busca la cámara main si no tiene cam asignada
        PlayerRB.freezeRotation = true; //congelar rotación de rigid body
        speedcontainer = speed;
        speedbase=speed;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TaskCollider= GameObject.FindGameObjectWithTag("TaskPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGrounded();
        if (playerOcupado)
        {
            speed = 0;
        }
        else
        {
            speed = speedcontainer;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto está en la layer deseada
        if (((1 << other.gameObject.layer) & taskLayer) != 0)
        {
            playerCerca = true;
            Debug.Log("Jugador entró en el área de Task (por layer)");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & taskLayer) != 0)
        {
            playerCerca = false;
            Debug.Log("Jugador salió del área de Task (por layer)");
        }
    }



    private void OnDrawGizmosSelected()
    {
        if(GroundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(GroundCheck.position, groundCheckRadius);
        }
    }

    private void FixedUpdate()
    {
        TaskDetector = Physics.CheckSphere(this.gameObject.transform.position, TaskDetectorRad, interactablesLayer);

        HandleMovement();
        HandleRotation();
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
        PlayerRB.linearVelocity = new Vector3(moveDireccion.x *speed, PlayerRB.linearVelocity.y, moveDireccion.z *speed);
    }

    void HandleRotation()
    {
        //si no existe imput, cerramos esta aplicación para ahorrar memoria
        if(moveImput == Vector2.zero) return;

        //hay que revisar la dirección de movimiento actual del rigidbody
        Vector3 moveDirection = new Vector3(PlayerRB.linearVelocity.x, 0, PlayerRB.linearVelocity.z);
        if (moveDirection == Vector3.zero ) return;

        //almacenar la dirección del personaje según la dirección a la que se está moviendo
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection); //si la dirección cambia bruscamente gira solo a esa dirección
        
        //se aplica el giro
        //suavizar el efecto del giro mediante interpolación de angulos
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);//el fixed no es necesario pero es una capa de seguridad
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, groundCheckRadius, groundLayer);

        //elemento de visualización en editor opcional

    }
    #region sprint
    Coroutine sprintCoroutine;
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (energeticas <= 0 || isSprinting) return;

            isSprinting = true;
            speedcontainer = sprintspeed;

            sprintCoroutine = StartCoroutine(StopSprintCoroutine());
        }

        if (context.canceled)
        {
            StopSprint();
        }
    }

    void StopSprint()
    {
        if (!isSprinting) return;

        isSprinting = false;
        speedcontainer = speedbase;

        if (sprintCoroutine != null)
        {
            StopCoroutine(sprintCoroutine);
            sprintCoroutine = null;
        }

        energeticas--;
    }

    IEnumerator StopSprintCoroutine()
    {
        yield return new WaitForSeconds(sprinttime);
        StopSprint();
    }
    #endregion
    #region imput methods
    public void OnMove (InputAction.CallbackContext context) //context bontón físico
    {
        if (!playerOcupado)
        {
            moveImput = context.ReadValue<Vector2>();
        }
            

    }
    
    #endregion


}
