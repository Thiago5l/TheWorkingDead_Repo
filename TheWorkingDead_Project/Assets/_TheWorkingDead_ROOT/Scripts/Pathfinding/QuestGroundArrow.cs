using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class QuestGroundArrowView : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private TareasAleatorias tareas;

    [Header("Configuración Flecha")]
    [SerializeField] private float heightAboveGround = 0.2f;
    [SerializeField] private float showDistance = 2f;
    [SerializeField] private float maxShowDistance = 8f;
    [SerializeField] private float navMeshSampleRadius = 2f;
    [SerializeField] private float offsetFromPlayer = 1f;
    [SerializeField] private float lineWidth = 0.2f;

    private LineRenderer lineRenderer;
    private NavMeshPath path;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        path = new NavMeshPath();

        // Configuración LineRenderer
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, lineWidth);

        // Esquinas suaves, extremos rectos
        lineRenderer.numCornerVertices = 5;
        lineRenderer.numCapVertices = 0;

        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // Solo mostrar mientras Tab está presionado
        if (!Input.GetKey(KeyCode.Tab))
        {
            lineRenderer.enabled = false;
            return;
        }

        if (tareas == null || tareas.OrdenTareas.Count == 0)
        {
            lineRenderer.enabled = false;
            return;
        }

        Transform targetTask = GetNextActiveTask();
        if (targetTask == null)
        {
            lineRenderer.enabled = false; // todas completadas
            return;
        }

        float distanceToTask = Vector3.Distance(playerTransform.position, targetTask.position);

        // Mostrar solo dentro del rango permitido
        if (distanceToTask < showDistance || distanceToTask > maxShowDistance)
        {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;

        // Dirección desde el jugador hacia la tarea
        Vector3 dirToTask = (targetTask.position - playerTransform.position).normalized;

        // Posición de inicio un poco delante del jugador hacia la tarea
        Vector3 startPos = playerTransform.position + dirToTask * offsetFromPlayer;
        startPos.y += heightAboveGround;

        // Ajustar posición al NavMesh más cercano si fuera del NavMesh
        NavMeshHit hitStart;
        if (NavMesh.SamplePosition(playerTransform.position, out hitStart, navMeshSampleRadius, NavMesh.AllAreas))
            startPos = hitStart.position + dirToTask * offsetFromPlayer + Vector3.up * heightAboveGround;

        Vector3 targetPos = targetTask.position;
        NavMeshHit hitEnd;
        if (NavMesh.SamplePosition(targetTask.position, out hitEnd, navMeshSampleRadius, NavMesh.AllAreas))
            targetPos = hitEnd.position;

        // Calcular path usando NavMesh
        bool pathFound = NavMesh.CalculatePath(startPos, targetPos, NavMesh.AllAreas, path);
        if (!pathFound || (path.status != NavMeshPathStatus.PathComplete && path.status != NavMeshPathStatus.PathPartial))
        {
            // Si no hay path, dibujar línea directa
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, targetPos + Vector3.up * heightAboveGround);
        }
        else
        {
            // Ajustar textura según longitud del path
            float pathLength = 0f;
            for (int i = 1; i < path.corners.Length; i++)
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            lineRenderer.material.SetTextureScale("_MainTex", new Vector2(pathLength, 1));

            // Dibujar path
            lineRenderer.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
                lineRenderer.SetPosition(i, path.corners[i] + Vector3.up * heightAboveGround);
        }
    }

    // Devuelve la siguiente tarea activa más cercana
    private Transform GetNextActiveTask()
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 playerPos = playerTransform.position;

        foreach (GameObject tareaObj in tareas.OrdenTareas)
        {
            if (tareaObj == null) continue;

            // Ignorar tareas inactivas (completadas)
            if (!tareaObj.activeInHierarchy) continue;

            float dist = Vector3.Distance(playerPos, tareaObj.transform.position);

            // Comprobar path parcial o completo
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(tareaObj.transform.position, out hit, navMeshSampleRadius, NavMesh.AllAreas))
                continue;

            NavMeshPath testPath = new NavMeshPath();
            if (!NavMesh.CalculatePath(playerTransform.position, hit.position, NavMesh.AllAreas, testPath))
                continue;
            if (testPath.status != NavMeshPathStatus.PathComplete && testPath.status != NavMeshPathStatus.PathPartial)
                continue;

            if (dist < minDist)
            {
                minDist = dist;
                closest = tareaObj.transform;
            }
        }

        return closest;
    }
}
