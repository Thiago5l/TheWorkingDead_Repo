using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TareaDibujar : MonoBehaviour
{
    [Header("Dibujo")]
    public RectTransform zonaDibujo;
    public RectTransform saverLineas;
    public GameObject lineaGenerar;
    public DibujoUI DibujoUI;

    private Linea linea;

    [Header("Tiempo")]
    public Slider barraTiempo;
    public float tiempoMaximo = 10f;

    public float tiempoActual;
    public bool contandoTiempo;

    public bool perderTarea = false;

    void Start()
    {
        tiempoActual = tiempoMaximo;
        barraTiempo.value = 1f;
        contandoTiempo = false;
    }

    void Update()
    {

        if(DibujoUI.tareaCompletada)
        {
            contandoTiempo = false;
        }

        // Actualizar tiempo
        if (contandoTiempo)
        {
            tiempoActual -= Time.deltaTime;
            barraTiempo.value = tiempoActual / tiempoMaximo;

            if (tiempoActual <= 0f)
            {
                TiempoAgotado();
            }
        }

        // Inicio dibujo
        if (Input.GetMouseButtonDown(0))
        {
            if (!DentroDeZona(Input.mousePosition)) return;

            if (!contandoTiempo)
                contandoTiempo = true;

            GameObject lineaActual = Instantiate(lineaGenerar, saverLineas/*zonaDibujo*/);
            linea = lineaActual.GetComponent<Linea>();

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                zonaDibujo,
                Input.mousePosition,
                null,
                out pos
            );

            linea.IniciarLinea(pos);
        }

        //  Fin dibujo
        if (Input.GetMouseButtonUp(0))
        {
            linea = null;
        }

        //  Dibujar
        if (linea != null)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                zonaDibujo,
                Input.mousePosition,
                null,
                out pos
            );

            linea.DibujarLinea(pos);
        }
    }

    bool DentroDeZona(Vector2 mousePos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            zonaDibujo,
            mousePos
        );
    }

    void TiempoAgotado()
    {
        tiempoActual = 0f;
        barraTiempo.value = 0f;
        contandoTiempo = false;
        perderTarea = true; 

        Debug.Log(" Tiempo agotado");

        // Aquí puedes:
        // - cancelar la tarea
        // - borrar líneas
        // - cerrar la UI
        // - avisar al gestor de misiones
    }
}

//using UnityEngine;
//using UnityEngine.EventSystems;

//public class TareaDibujar : MonoBehaviour
//{
//    public RectTransform zonaDibujo;
//    public GameObject lineaGenerar;
//    public DibujoUI DibujoUI;
//    private Linea linea;

//    [Header("Tiempo")]
//    public Slider barraTiempo;
//    public float tiempoMaximo = 10f;

//    float tiempoActual;
//    bool contandoTiempo;

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            if (!DentroDeZona(Input.mousePosition)) return;

//            GameObject lineaActual = Instantiate(lineaGenerar, zonaDibujo);
//            linea = lineaActual.GetComponent<Linea>();

//            Vector2 pos;
//            RectTransformUtility.ScreenPointToLocalPointInRectangle(
//                zonaDibujo,
//                Input.mousePosition,
//                null,
//                out pos
//            );

//            linea.IniciarLinea(pos);
//        }

//        if (Input.GetMouseButtonUp(0))
//        {
//            linea = null;
//        }

//        if (linea != null)
//        {
//            Vector2 pos;
//            RectTransformUtility.ScreenPointToLocalPointInRectangle(
//                zonaDibujo,
//                Input.mousePosition,
//                null,
//                out pos
//            );

//            linea.DibujarLinea(pos);
//        }
//    }

//    bool DentroDeZona(Vector2 mousePos)
//    {
//        return RectTransformUtility.RectangleContainsScreenPoint(
//            zonaDibujo,
//            mousePos
//        );
//    }

    
//}