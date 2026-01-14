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
    public bool meandoDentro;

    private void Awake()
    {
        punteroRB = GetComponent<Rigidbody>();
        if (camTransform == null) camTransform = Camera.main.transform; //busca la cámara main si no tiene cam asignada
        punteroRB.freezeRotation = true; //congelar rotación de rigid body
        speedcontainer = speed;
        speedbase = speed;
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
        ray = new Ray(meandoGreg.transform.position, puntero.transform.position);
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
        Physics.Raycast(ray);
    }
}
