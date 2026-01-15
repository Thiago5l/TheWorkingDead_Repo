using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerControllerTutorial : MonoBehaviour
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
    public bool playerCerca;
    public GameObject TaskCollider;
    public LayerMask taskLayer;

    [Header("sprint parametres")]
    [SerializeField] public float sprintspeed = 10f;
    [SerializeField] public float sprinttime = 3f;
    public bool isSprinting;
    [SerializeField] EnergeticasUI energeticasUI;
    [SerializeField] Canvas EstaminaUI;
    [SerializeField] Image EstaminaFillImage;
    float sprintTimer;
    [SerializeField] Image EstaminaDelayedImage; // la barra roja
    [SerializeField] float delaySpeed = 2f; // velocidad con la que la barra roja sigue
    [SerializeField] public GameObject sprintVFX;

    [Header("snack parametres")]
    [SerializeField] Snacks_UI snacks_UI;
    [SerializeField] Image snackfill;
    [SerializeField] public float snackzombiedadspeed = 2;
    [SerializeField] public float snacktime = 0;
    [SerializeField] float snackTimer;
    [SerializeField] OviedadZombie obviedadZombie;
    [SerializeField] public bool snackusado = false;

    [Header("resources")]
    [SerializeField] public float energeticas = 1;
    [SerializeField] public int snacks = 1;
    [SerializeField] public float coins = 1;

    [Header("aires")]
    [SerializeField] public int airesapagados = 0;

    Rigidbody PlayerRB;//ref a rigid boddy
    Vector2 moveImput;//almacén imput mov
    bool isGrounded;//determina si estás tocando el suelo
    bool TaskDetector;

    [Header("tutorial")]
    public bool tutorialroom;
    public bool playedvendingmachine=false;
    #endregion

    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody>();
        if (camTransform == null) camTransform = Camera.main.transform; //busca la cámara main si no tiene cam asignada
        PlayerRB.freezeRotation = true; //congelar rotación de rigid body
        speedcontainer = speed;
        speedbase=speed;
        //sprintVFX.SetActive(false);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialroom = false;
        TaskCollider= GameObject.FindGameObjectWithTag("TaskPlayer");
        energeticasUI.SetEnergeticas((int)energeticas);
        snacks_UI.SetSnacks((int)snacks);
        EstaminaUI.enabled = false;
        snackTimer = snacktime;
        int snackIndex = snacks - 1; // si snacks = 3 -> índice = 2 (último)
        if (snackIndex >= 0 && snackIndex < snacks_UI.icons.Count)
            snackfill = snacks_UI.icons[snackIndex].GetComponent<Image>();
        else
            snackfill = null;
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

        if (isSprinting && EstaminaFillImage != null)
        {
            // Barra principal baja inmediatamente
            sprintTimer -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(sprintTimer / sprinttime);
            EstaminaFillImage.fillAmount = fillAmount;

            // Barra roja se interpola suavemente hacia la principal
            if (EstaminaDelayedImage != null)
            {
                EstaminaDelayedImage.fillAmount = Mathf.Lerp(
                    EstaminaDelayedImage.fillAmount,
                    fillAmount,
                    Time.deltaTime * delaySpeed
                );
            }
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
        if (other.gameObject.CompareTag("Roomcollider"))
        { tutorialroom=true; }
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
        if (GroundCheck != null)
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
        if (moveImput == Vector2.zero) return;

        //hay que revisar la dirección de movimiento actual del rigidbody
        Vector3 moveDirection = new Vector3(PlayerRB.linearVelocity.x, 0, PlayerRB.linearVelocity.z);
        if (moveDirection == Vector3.zero) return;

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
            EstaminaUI.enabled = true;
            sprintVFX.SetActive(true);
            isSprinting = true;
            speedcontainer = sprintspeed;
            sprintTimer = sprinttime; // inicializa el temporizador

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

        EstaminaUI.enabled = false;
        sprintVFX.SetActive(false);
        EstaminaFillImage.fillAmount = 1f;
        if (EstaminaDelayedImage != null)
            EstaminaDelayedImage.fillAmount = 1f;

        energeticas--;
        energeticasUI.SetEnergeticas((int)energeticas);
    }


    IEnumerator StopSprintCoroutine()
    {
        yield return new WaitForSeconds(sprinttime);
        StopSprint();
    }
    #endregion
    #region snack
    public void snack(InputAction.CallbackContext context)
    {
            if (!context.performed) return;
            if (snackusado || snacks <= 0) return;

            snackusado = true;

            // Obtener el índice del último snack disponible
            int snackIndex = snacks - 1; // si snacks = 3 -> índice = 2 (último)
        if (snackIndex >= 0 && snackIndex < snacks_UI.icons.Count)
            snackfill = snacks_UI.icons[snackIndex].GetComponent<Image>();
        else
            snackfill = null;

        snackTimer = snacktime;
            StartCoroutine(SnackCoroutine());
            /////////
        if (!context.performed) return;
        {
            Debug.Log("snackusado");

            if (!snackusado && snacks>0)
            {
                snackTimer=snacktime;
                Debug.Log("snackconsumido");
                snackusado = true;
                StartCoroutine(SnackCoroutine());
            }
        }
    }
    IEnumerator SnackCoroutine()
    {
        snackTimer = snacktime;
        obviedadZombie.ZombiedadSpeed = snackzombiedadspeed;

        // Mientras dure el snack
        while (snackTimer > 0f)
        {
            snackTimer -= Time.deltaTime;
            if (snackfill != null)
                snackfill.fillAmount = snackTimer / snacktime;
            yield return null;
        }

        // Reset del efecto
        obviedadZombie.resetspeed();
        snacks--;
        snacks_UI.SetSnacks((int)snacks);
        snackusado = false;

        // seguridad
        if (snackfill != null)
            snackfill.fillAmount = 0f;
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
