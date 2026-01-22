using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 posicionInicial;
    private CanvasGroup canvasGroup;

    private ContenedorReciclajeUI contenedorActual = null;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
            Debug.LogError("DragUI debe estar dentro de un Canvas.");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        posicionInicial = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        ContenedorReciclajeUI nuevoContenedor = null;

        foreach (var r in results)
        {
            nuevoContenedor = r.gameObject.GetComponent<ContenedorReciclajeUI>();
            if (nuevoContenedor == null)
                nuevoContenedor = r.gameObject.GetComponentInParent<ContenedorReciclajeUI>();
            if (nuevoContenedor != null) break;
        }

        if (nuevoContenedor != contenedorActual)
        {
            if (contenedorActual != null)
                contenedorActual.RestaurarTamano();

            contenedorActual = nuevoContenedor;

            if (contenedorActual != null)
                contenedorActual.Agrandar();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        bool correcto = false;

        var contenedorLocal = contenedorActual;
        contenedorActual = null;

        if (contenedorLocal != null)
        {
            ObjetoReciclable objReciclable = GetComponent<ObjetoReciclable>();
            if (objReciclable != null)
            {
                if (objReciclable.tipoCorrecto == contenedorLocal.tipoAceptado)
                {
                    contenedorLocal.Felicidad();
                    Destroy(gameObject);
                    correcto = true;
                }
                else
                {
                    contenedorLocal.Tremble();
                }
            }

            contenedorLocal.RestaurarTamano(); // ← FIX CLAVE
        }

        if (!correcto)
            rectTransform.anchoredPosition = posicionInicial;
    }

}
