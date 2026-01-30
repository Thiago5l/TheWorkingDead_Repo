using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ZonaLineaUI : MonoBehaviour
{
    public RectTransform zona;
    [Range(0f, 1f)] public float porcentajeNecesario = 0.7f;
    public bool completada;
    [SerializeField] private GameObject imagenTrazo;
    public float progreso = 0;
    public void Validar(List<Vector2> puntosScreen)
    {
        if (completada || puntosScreen.Count == 0)
            return;

        int dentro = 0;

        foreach (Vector2 screenPos in puntosScreen)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                zona, screenPos))
            {
                dentro++;
            }
        }

        progreso = (float)dentro / puntosScreen.Count;

        Debug.Log($"{zona.name} progreso: {progreso:P0}");

        if (progreso >= porcentajeNecesario)
        {
            completada = true;
            imagenTrazo.SetActive(false);
            Debug.Log($"{zona.name} COMPLETADA");
        }
    }
    public void Resetear()
    {
        imagenTrazo.SetActive(true);
        progreso = 0f;
        completada = false;

    }

}