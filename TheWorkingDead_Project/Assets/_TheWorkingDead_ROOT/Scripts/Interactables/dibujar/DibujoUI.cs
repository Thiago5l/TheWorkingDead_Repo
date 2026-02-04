using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DibujoUI : MonoBehaviour
{
    [Header("Dibujo")]
    public RawImage rawImage;
    public int texturaAncho = 512;
    public int texturaAlto = 512;
    public Color colorLinea = Color.black;
    public int grosor = 4;

    [Header("Validación")]
    public RectTransform rectDibujo;
    public ZonaLineaUI[] zonas;
    [SerializeField] private float progresoPorcentage = 0;
    [SerializeField] private Slider progresoSlide;
    public bool tareaCompletada = false;

    Texture2D textura;
    Vector2 ultimoPunto;
    //List<Vector2> puntos = new List<Vector2>();

    List<Vector2> puntosWorld = new List<Vector2>();

    void Start()
    {
        progresoSlide.value = progresoPorcentage;
        progresoSlide.maxValue = zonas[1].porcentajeNecesario;
        textura = new Texture2D(texturaAncho, texturaAlto, TextureFormat.RGBA32, false);
        textura.filterMode = FilterMode.Point;

        Color[] clear = new Color[texturaAncho * texturaAlto];
        for (int i = 0; i < clear.Length; i++)
            clear[i] = Color.clear;

        textura.SetPixels(clear);
        textura.Apply();
        rawImage.texture = textura;
    }

    void Update()
    {

        float suma = 0f;

        foreach (var zona in zonas)
        {
            suma += zona.progreso;
        }

        progresoPorcentage = suma / zonas.Length;
        progresoSlide.value = progresoPorcentage;
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                rectDibujo, Input.mousePosition)) return;

            puntosWorld.Clear();

            ultimoPunto = ObtenerPosicion(); // textura (para dibujar)
            puntosWorld.Add(Input.mousePosition); // world/screen
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 p = ObtenerPosicion(); // textura
            DibujarLinea(ultimoPunto, p);
            ultimoPunto = p;

            puntosWorld.Add(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            ValidarZonas();
        }
    }
    public void LimpiarDibujo()
    {
        Color[] clear = new Color[texturaAncho * texturaAlto];
        for (int i = 0; i < clear.Length; i++)
            clear[i] = Color.clear;

        textura.SetPixels(clear);
        textura.Apply();

        progresoPorcentage = 0f;
        progresoSlide.value = 0f;
        tareaCompletada = false;

        foreach (var zona in zonas)
            zona.Resetear(); 
    }

    void ValidarZonas()
    {
        //float suma = 0f;

        //foreach (var zona in zonas)
        //{
        //    suma += zona.progreso;
        //}

        //progresoPorcentage = suma / (zonas.Length - 1);
        //progresoSlide.value = progresoPorcentage;
        foreach (var zona in zonas)
            zona.Validar(puntosWorld);

        bool completa = true;
        foreach (var z in zonas)
            if (!z.completada) completa = false;

        if (completa)
            Debug.Log(" TAREA COMPLETADA");
        tareaCompletada = completa;
    }

    Vector2 ObtenerPosicion()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectDibujo,
            Input.mousePosition,
            null,
            out Vector2 localPos
        );

        Rect rect = rectDibujo.rect;

        float x = (localPos.x - rect.x) / rect.width * texturaAncho;
        float y = (localPos.y - rect.y) / rect.height * texturaAlto;

        return new Vector2(x, y);
    }

    void DibujarLinea(Vector2 a, Vector2 b)
    {
        int pasos = Mathf.CeilToInt(Vector2.Distance(a, b));
        for (int i = 0; i < pasos; i++)
        {
            Vector2 p = Vector2.Lerp(a, b, i / (float)pasos);
            DibujarPunto((int)p.x, (int)p.y);
        }
        textura.Apply();
    }

    void DibujarPunto(int x, int y)
    {
        for (int i = -grosor; i <= grosor; i++)
            for (int j = -grosor; j <= grosor; j++)
            {
                int px = x + i;
                int py = y + j;
                if (px >= 0 && px < texturaAncho && py >= 0 && py < texturaAlto)
                    textura.SetPixel(px, py, colorLinea);
            }
    }
}

//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class DibujoUI : MonoBehaviour
//{
//    public RawImage rawImage;
//    public int texturaAncho = 512;
//    public int texturaAlto = 512;
//    public Color colorLinea = Color.black;
//    [Range(0f, 100f)]
//    public int grosor = 4;

//    private Texture2D textura;
//    private Vector2 ultimoPunto;

//    public List<Vector2> puntosDibujados = new List<Vector2>();
//    //public ValidadorDibujoUI validador;

//    void Start()
//    {
//        textura = new Texture2D(texturaAncho, texturaAlto, TextureFormat.RGBA32, false);
//        textura.filterMode = FilterMode.Point;

//        Color[] pixeles = new Color[texturaAncho * texturaAlto];
//        for (int i = 0; i < pixeles.Length; i++)
//            pixeles[i] = Color.clear;

//        textura.SetPixels(pixeles);
//        textura.Apply();

//        rawImage.texture = textura;
//        //validador.dibujoJugador = textura;
//    }
//    //void Start()
//    //{
//    //    textura = new Texture2D(texturaAncho, texturaAlto, TextureFormat.RGBA32, false);
//    //    textura.filterMode = FilterMode.Point;

//    //    Color[] pixeles = new Color[texturaAncho * texturaAlto];
//    //    for (int i = 0; i < pixeles.Length; i++)
//    //        pixeles[i] = Color.clear;

//    //    textura.SetPixels(pixeles);
//    //    textura.Apply();

//    //    rawImage.texture = textura;
//    //}

//    void Update()
//    {

//        if (Input.GetMouseButtonDown(0))
//        {
//            puntosDibujados.Clear();
//            ultimoPunto = ObtenerPosicion();
//            puntosDibujados.Add(ultimoPunto);
//        }

//        if (Input.GetMouseButton(0))
//        {
//            Vector2 puntoActual = ObtenerPosicion();
//            DibujarLinea(ultimoPunto, puntoActual);
//            puntosDibujados.Add(puntoActual);
//            ultimoPunto = puntoActual;
//        }

//        //if (Input.GetMouseButtonDown(0))
//        //{
//        //    if (!RectTransformUtility.RectangleContainsScreenPoint(
//        //        rawImage.rectTransform, Input.mousePosition)) return;

//        //    ultimoPunto = ObtenerPosicion();
//        //}

//        //if (Input.GetMouseButton(0))
//        //{
//        //    Vector2 puntoActual = ObtenerPosicion();
//        //    DibujarLinea(ultimoPunto, puntoActual);
//        //    ultimoPunto = puntoActual;
//        //}
//        //if (Input.GetMouseButtonUp(0))
//        //{
//        //    validador.Validar();
//        //}
//    }

//    Vector2 ObtenerPosicion()
//    {
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(
//            rawImage.rectTransform,
//            Input.mousePosition,
//            null,
//            out Vector2 localPos
//        );

//        Rect rect = rawImage.rectTransform.rect;

//        float x = (localPos.x - rect.x) / rect.width * texturaAncho;
//        float y = (localPos.y - rect.y) / rect.height * texturaAlto;

//        return new Vector2(x, y);
//    }

//    void DibujarLinea(Vector2 a, Vector2 b)
//    {
//        int pasos = Mathf.CeilToInt(Vector2.Distance(a, b));
//        for (int i = 0; i < pasos; i++)
//        {
//            Vector2 p = Vector2.Lerp(a, b, i / (float)pasos);
//            DibujarPunto((int)p.x, (int)p.y);
//        }
//        textura.Apply();
//    }

//    void DibujarPunto(int x, int y)
//    {
//        for (int i = -grosor; i <= grosor; i++)
//            for (int j = -grosor; j <= grosor; j++)
//            {
//                int px = x + i;
//                int py = y + j;

//                if (px >= 0 && px < texturaAncho && py >= 0 && py < texturaAlto)
//                    textura.SetPixel(px, py, colorLinea);
//            }
//    }
//}