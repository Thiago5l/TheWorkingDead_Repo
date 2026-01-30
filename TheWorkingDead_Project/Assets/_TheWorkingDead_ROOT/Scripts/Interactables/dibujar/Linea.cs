using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Linea : MonoBehaviour
{
    [SerializeField]LineRenderer linea;
    [SerializeField]List<Vector2> puntos;
    [SerializeField]Vector2 ultimoPunto;
    [Range(0f, 100f)]
    [SerializeField] private float grosorLinea = 5f;


    [SerializeField] float distanciaMinima = 5f;

    void Awake()
    {
        linea = GetComponent<LineRenderer>();
        linea.useWorldSpace = false;
        linea.widthMultiplier = grosorLinea;
    }

    public void IniciarLinea(Vector2 puntoInicial)
    {
        puntos = new List<Vector2>();
        DibujarPunto(puntoInicial);
    }

    public void DibujarLinea(Vector2 punto)
    {
        if (Vector2.Distance(ultimoPunto, punto) > distanciaMinima)
        {
            DibujarPunto(punto);
        }
    }

    void DibujarPunto(Vector2 punto)
    {
        puntos.Add(punto);
        linea.positionCount = puntos.Count;
        linea.SetPosition(puntos.Count - 1, punto);
        ultimoPunto = punto;
    }
    public List<Vector2> ObtenerPuntos()
    {
        return puntos;
    }
}