using UnityEngine;
using UnityEngine.UI;

public class MearUI : MonoBehaviour
{


    // -----------------------------
    // UI
    // -----------------------------
    [Header("UI References")]
    [SerializeField] private RectTransform punteroRT;
    [SerializeField] private LineRenderer line;
    public Slider barraMear;

    // -----------------------------
    // WORLD
    // -----------------------------
    [Header("World References")]
    public Transform meandoGreg;

    // -----------------------------
    // MOVIMIENTO
    // -----------------------------
    [Header("Movimiento")]
    [SerializeField] private float speed = 300f;
    [SerializeField] private float autoForce = 150f;
    [SerializeField] private float tiempoDesvio = 1.5f;
    [SerializeField] private float limiteX = 400f;

    // -----------------------------
    // PARÁBOLA
    // -----------------------------
    [Header("Parábola")]
    [SerializeField] private int segments = 30;
    [SerializeField] private float height = 2f;

    // -----------------------------
    // BARRA
    // -----------------------------
    [Header("Barra")]
    [SerializeField] private float tiempoBarra = 0.5f;

    // -----------------------------
    // ESTADO
    // -----------------------------
    private Vector2 punteroPos;
    private float direccionAuto = 1f;
    private float timerDesvio;
    private float timerBarra;
    public bool meandoDentro;

    // -----------------------------
    // UNITY
    // -----------------------------
    private void Start()
    {
        punteroPos = punteroRT.anchoredPosition;
    }

    private void Update()
    {
        MoverPuntero();
        //ControlDesvio();
        ControlBarra();
        DibujarParabola();
        if (Input.anyKey)
        {
            Debug.Log("Hay input");
        }
    }

    // -----------------------------
    // MOVIMIENTO PUNTERO
    // -----------------------------
    private void MoverPuntero()
    {
        float input = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            input = -1f;
        if (Input.GetKey(KeyCode.RightArrow))
            input = 1f;

        punteroPos.x += input * speed * Time.deltaTime;
        punteroPos.x += direccionAuto * autoForce * Time.deltaTime;

        punteroPos.x = Mathf.Clamp(punteroPos.x, -limiteX, limiteX);

        punteroRT.anchoredPosition = punteroPos;
    }

    // -----------------------------
    // DESVÍO AUTOMÁTICO
    // -----------------------------
    //private void ControlDesvio()
    //{
    //    timerDesvio += Time.deltaTime;

    //    if (timerDesvio >= tiempoDesvio)
    //    {
    //        direccionAuto = Random.value > 0.5f ? 1f : -1f;
    //        timerDesvio = 0f;
    //    }
    //}

    // -----------------------------
    // BARRA DE PROGRESO
    // -----------------------------
    private void ControlBarra()
    {
        timerBarra += Time.deltaTime;
        if (timerBarra < tiempoBarra) return;

        timerBarra = 0f;

        float delta = barraMear.maxValue * 0.02f;
        barraMear.value += meandoDentro ? delta : -delta;
    }

    // -----------------------------
    // PARÁBOLA VISUAL
    // -----------------------------
    private void DibujarParabola()
    {
        if (!line || !meandoGreg) return;

        line.positionCount = segments + 1;

        Vector3 start = meandoGreg.position;

        Vector3 end;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            punteroRT,
            punteroRT.position,
            Camera.main,
            out end
        );

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * height;
            line.SetPosition(i, pos);
        }
    }

    // -----------------------------
    // ZONA RETRETE
    // -----------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RetreteZonaMear"))
            meandoDentro = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RetreteZonaMear"))
            meandoDentro = false;
    }

    //[Header("UI References")]
    //public RectTransform punteroRT;
    //public LineRenderer line;

    //[Header("World References")]
    //public Transform meandoGreg;
    //public Transform retreteZona;

    //[Header("Movimiento")]
    //public float speed = 300f;
    //public float autoForce = 150f;
    //public float tiempoDesvio = 1.5f;

    //[Header("Parábola")]
    //public int segments = 30;
    //public float height = 2f;

    //[Header("Barra")]
    //public Slider BarraMear;
    //public float tiempoBarra = 0.5f;

    //public bool meandoDentro;
    //float direccionAuto = 1f;
    //float timerDesvio;
    //float timerBarra;

    //Vector2 punteroPos;


    //public GameObject objetoRetrete;
    //[SerializeField] private TareaMear scriptMear;

    //public GameObject puntero;
    //public GameObject retreteUI;

    //public bool izquierdaDerecha;
    //private Ray ray;
    ///// <summary>
    ///// //////////////////////////////////////////////////////////////////////--------
    ///// </summary>

    //private void Awake()
    //{
    //    punteroRT = puntero.GetComponent<RectTransform>();

    //}
    //void Start()
    //{
    //    punteroPos = punteroRT.anchoredPosition;
    //    meandoDentro = false;
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("RetreteZonaMear"))
    //    {
    //        meandoDentro = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("RetreteZonaMear"))
    //    {
    //        meandoDentro = false;
    //    }
    //}

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    ////Update is called once per frame
    //void Update()
    //{
    //    MoverPuntero();
    //    ControlDesvio();
    //    ControlBarra();
    //    DrawParabola();

    //}
    //// -----------------------------
    //// MOVIMIENTO UI
    //// -----------------------------
    //void MoverPuntero()
    //{
    //    float input = 0f;

    //    if (Input.GetKey(KeyCode.LeftArrow))
    //        input = -1f;
    //    if (Input.GetKey(KeyCode.RightArrow))
    //        input = 1f;

    //    punteroPos.x += input * speed * Time.deltaTime;
    //    punteroPos.x += direccionAuto * autoForce * Time.deltaTime;

    //    //LIMITES
    //    punteroPos.x = Mathf.Clamp(punteroPos.x, -400f, 400f);

    //    punteroRT.anchoredPosition = punteroPos;



    //}

    //// -----------------------------
    //// DESVIACIÓN AUTOMÁTICA
    //// -----------------------------
    //void ControlDesvio()
    //{
    //    timerDesvio += Time.deltaTime;

    //    if (timerDesvio >= tiempoDesvio)
    //    {
    //        direccionAuto *= Random.Range(-1, 1);
    //        timerDesvio = 0f;
    //    }
    //}

    //// -----------------------------
    //// CONTROL DE BARRA
    //// -----------------------------
    //void ControlBarra()
    //{
    //    timerBarra += Time.deltaTime;

    //    if (timerBarra < tiempoBarra)
    //        return;

    //    timerBarra = 0f;

    //    if (meandoDentro)
    //    {
    //        BarraMear.value += BarraMear.maxValue * 0.02f;
    //    }
    //    else
    //    {
    //        BarraMear.value -= BarraMear.maxValue * 0.02f;
    //    }
    //}

    //// -----------------------------
    //// PARÁBOLA VISUAL
    //// -----------------------------
    //void DrawParabola()
    //{
    //    if (line == null || meandoGreg == null)
    //        return;

    //    line.positionCount = segments + 1;

    //    Vector3 start = meandoGreg.position;
    //    Vector3 end;
    //    RectTransformUtility.ScreenPointToWorldPointInRectangle(
    //        punteroRT,
    //        punteroRT.position,
    //        Camera.main,
    //        out end
    //    );

    //    for (int i = 0; i <= segments; i++)
    //    {
    //        float t = i / (float)segments;
    //        Vector3 pos = Vector3.Lerp(start, end, t);
    //        pos.y += Mathf.Sin(t * Mathf.PI) * height;

    //        line.SetPosition(i, pos);
    //    }
    //}



    ////IEnumerator BajarBarra()
    ////{
    ////    yield return new WaitForSeconds(tiempoBarra);
    ////    BarraMear.value -= BarraMear.maxValue * 0.02f;
    ////}
    ////IEnumerator SubirBarra()
    ////{
    ////    yield return new WaitForSeconds(tiempoBarra);
    ////    BarraMear.value += BarraMear.maxValue * 0.02f;
    ////}
    ////IEnumerator Desviar()
    ////{
    ////    if (izquierdaDerecha)
    ////    {
    ////        //izquierdaDerecha = false;
    ////    }
    ////    //new WaitForSeconds(tiempoDesvio);
    ////    if (!izquierdaDerecha)
    ////    {
    ////        //izquierdaDerecha = true;
    ////    }
    ////    yield return new WaitForSeconds(tiempoDesvio);
    ////    if(izquierdaDerecha)
    ////    {
    ////        izquierdaDerecha = false;
    ////    }
    ////    else
    ////    {
    ////        izquierdaDerecha = true;
    ////    }
    ////}

}
