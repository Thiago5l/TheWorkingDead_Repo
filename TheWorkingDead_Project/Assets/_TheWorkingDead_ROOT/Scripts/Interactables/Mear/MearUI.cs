using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MearUI : MonoBehaviour
{
    [Header("Editor References")]
    [SerializeField] Transform camTransform; //ref  transform cámara

    [Header("Movement Parametres")]
    [SerializeField] float speed = 10f;
    Vector2 moveInput;//almacén imput mov
    Rigidbody punteroRB;//ref a rigid boddy



    public GameObject objetoRetrete;
    [SerializeField] private TareaMear scriptMear;
    public GameObject meandoGreg;
    public GameObject puntero;
    public GameObject retreteUI;
    public float tiempoBarra;
    public Slider BarraMear;
    public float tiempoDesvio;
    public bool izquierdaDerecha;
    private Ray ray;
    public bool meandoDentro;

    private void Awake()
    {
        punteroRB = GetComponent<Rigidbody>();
        if (camTransform == null) camTransform = Camera.main.transform; //busca la cámara main si no tiene cam asignada
        punteroRB.freezeRotation = true; //congelar rotación de rigid body
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RetreteZonaMear"))
        {
            meandoDentro = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("RetreteZonaMear"))
        {
            meandoDentro = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scriptMear = objetoRetrete.GetComponent<TareaMear>();
        meandoDentro = false; 
    }
    //Update is called once per frame
    void Update()
    {
        StartCoroutine(Desviar());
        if (meandoDentro)
        {
            Debug.Log("Dentro");
            StopCoroutine(BajarBarra());
            StartCoroutine(SubirBarra());
        }
        if (!meandoDentro)
        {
            Debug.Log("Fuera");
            StopCoroutine(SubirBarra());
            StartCoroutine(BajarBarra());
        }
        
        ray = new Ray(meandoGreg.transform.position, puntero.transform.position);
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        Physics.Raycast(ray);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        punteroRB.linearVelocity = new Vector2(moveInput.x * speed, punteroRB.linearVelocity.y);
    }
    IEnumerator BajarBarra()
    {
        yield return new WaitForSeconds(tiempoBarra);
        BarraMear.value -= BarraMear.maxValue * 0.02f;
    }
    IEnumerator SubirBarra()
    {
        yield return new WaitForSeconds(tiempoBarra);
        BarraMear.value += BarraMear.maxValue * 0.02f;
    }
    IEnumerator Desviar()
    {
        if (izquierdaDerecha)
        {
            punteroRB.AddForce(Vector3.right * (speed/2), ForceMode.VelocityChange);
            izquierdaDerecha = false;
        }
        if (!izquierdaDerecha)
        {
            punteroRB.AddForce(Vector3.left * (speed/2), ForceMode.VelocityChange);
            izquierdaDerecha = true;
        }
        yield return new WaitForSeconds(tiempoDesvio);
        if(izquierdaDerecha)
        {
         izquierdaDerecha = false;
        }
        else
        {
         izquierdaDerecha = true;
        }
    }

}
