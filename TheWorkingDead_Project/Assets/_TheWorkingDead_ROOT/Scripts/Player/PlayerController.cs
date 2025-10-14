using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{


    #region General Variables
    [Header("Editor References")]
    [SerializeField] Transform camTransform; //ref  transform c�mara


    [Header("Movement Parametres")]
    [SerializeField] float speed = 10f;
    [SerializeField] float rotSpeed = 15f;


    [Header("jump parametres")]
    [SerializeField] Transform GroundCheck; //ref posici�n desde la que se detecta el suelo
    [SerializeField] float groundCheckRadius = 0.2f; //rando detecci�n
    [SerializeField] LayerMask groundLayer; //capa detecci�n suelo


    //variables referencia propias o internas


    Rigidbody PlayerRB;//ref a rigid boddy
    Vector2 moveImput;//almac�n imput mov
    bool isGrounded;//determina si est�s tocando el suelo
    #endregion

    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody>();
        if(camTransform == null) camTransform = Camera.main.transform; //busca la c�mara main si no tiene cam asignada
        PlayerRB.freezeRotation = true; //congelar rotaci�n de rigid body
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGrounded();
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
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        //almacenar direcci�n z + x de la c�mara
        Vector3 cameraForward = Camera.main.transform.forward;//almacena el origen frontal de la c�mara
        Vector3 cameraRight = Camera.main.transform.right;//almacena el origen lateral de la c�mara

        //anular la orientaci�n e y antes del c�lculo de la orientaci�n de la c�mara aplicada a l movimiento
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize(); //el valor del float es m�ximo 1 por lo que no afecta a la multiplicaci�n de velocidad
        cameraRight.Normalize(); //el valor del float es m�ximo 1 por lo que no afecta a la multiplicaci�n de velocidad

        //se calcula y almacena la direcci�n x/z teniendo en cuenta la c�mara por el imput
        Vector3 moveDireccion = (cameraForward * moveImput.y + cameraRight * moveImput.x).normalized;
        //una vez tenemos la direcci�n +el imput se lo aplicamos al motor de aceleraci�n del rigidbody
        //todo esto sin afectar al eje y, porque eso se encargar� el salto
        PlayerRB.linearVelocity = new Vector3(moveDireccion.x *speed, PlayerRB.linearVelocity.y, moveDireccion.z *speed);
    }

    void HandleRotation()
    {
        //si no existe imput, cerramos esta aplicaci�n para ahorrar memoria
        if(moveImput == Vector2.zero) return;

        //hay que revisar la direcci�n de movimiento actual del rigidbody
        Vector3 moveDirection = new Vector3(PlayerRB.linearVelocity.x, 0, PlayerRB.linearVelocity.z);
        if (moveDirection == Vector3.zero ) return;

        //almacenar la direcci�n del personaje seg�n la direcci�n a la que se est� moviendo
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection); //si la direcci�n cambia bruscamente gira solo a esa direcci�n
        
        //se aplica el giro
        //suavizar el efecto del giro mediante interpolaci�n de angulos
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);//el fixed no es necesario pero es una capa de seguridad
    }

    void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, groundCheckRadius, groundLayer);

        //elemento de visualizaci�n en editor opcional

    }

    #region imput methods
    public void OnMove (InputAction.CallbackContext context) //context bont�n f�sico
    {
        moveImput = context.ReadValue<Vector2>();

    }
    
    #endregion


}
