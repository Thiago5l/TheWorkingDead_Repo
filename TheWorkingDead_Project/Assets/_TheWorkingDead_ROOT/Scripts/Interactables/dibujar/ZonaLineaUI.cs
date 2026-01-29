using UnityEngine;
using System.Collections.Generic;

public class ZonaLineaUI : MonoBehaviour
{
    public Collider2D area;
    [Range(0f, 1f)] public float porcentajeNecesario = 0.7f;
    public bool completada;

    public void Validar(List<Vector2> puntos, RectTransform dibujoRect)
    {
        if (completada) return;

        int dentro = 0;

        foreach (Vector2 p in puntos)
        {
            Vector2 worldPos = dibujoRect.TransformPoint(p);
            if (area.OverlapPoint(worldPos))
                dentro++;
        }

        float progreso = (float)dentro / puntos.Count;
        if (progreso >= porcentajeNecesario)
        {
            completada = true;
            Debug.Log($"{name} completada");
        }
    }
}