using UnityEngine;
using UnityEngine.EventSystems;

public class TareaDibujar : MonoBehaviour
{
    public RectTransform zonaDibujo;
    public GameObject lineaGenerar;

    private Linea linea;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!DentroDeZona(Input.mousePosition)) return;

            GameObject lineaActual = Instantiate(lineaGenerar, zonaDibujo);
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

        if (Input.GetMouseButtonUp(0))
        {
            linea = null;
        }

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
}