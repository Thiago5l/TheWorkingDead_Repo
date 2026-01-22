using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class QuestGroundArrowSmooth : MonoBehaviour
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

    [Header("Animación Textura")]
    [SerializeField] private float textureScrollSpeed = 2f; // velocidad base
    [SerializeField] private float tileSizeInWorld = 1f;    // tamaño de cada flecha
    [SerializeField] private float segmentLength = 0.5f;    // longitud de subsegmentos

    [Header("Corrección Movimiento Jugador")]
    [SerializeField] private float playerOffsetFactor = 0.2f; // cuánto afecta el movimiento del jugador a la animación

    private LineRenderer lineRenderer;
    private NavMeshPath path;
    private Material lineMat;
    private float cumulativeOffset = 0f;
    private Vector3 lastPlayerPos;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        path = new NavMeshPath();

        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, lineWidth);
        lineRenderer.numCornerVertices = 5;
        lineRenderer.numCapVertices = 0;

        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;

        lineMat = lineRenderer.material;
        lineMat.SetTextureScale("_BaseMap", new Vector2(1f, 1f));

        lastPlayerPos = playerTransform.position;
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.Tab) || tareas == null || tareas.OrdenTareas.Count == 0)
        {
            lineRenderer.enabled = false;
            return;
        }

        Transform targetTask = GetNextActiveTask();
        if (targetTask == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        float distanceToTask = Vector3.Distance(playerTransform.position, targetTask.position);
        if (distanceToTask < showDistance || distanceToTask > maxShowDistance)
        {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;

        // Calcular path
        Vector3 dirToTask = (targetTask.position - playerTransform.position).normalized;
        Vector3 startPos = playerTransform.position + dirToTask * offsetFromPlayer;
        startPos.y += heightAboveGround;

        NavMeshHit hitStart;
        if (NavMesh.SamplePosition(playerTransform.position, out hitStart, navMeshSampleRadius, NavMesh.AllAreas))
            startPos = hitStart.position + dirToTask * offsetFromPlayer + Vector3.up * heightAboveGround;

        Vector3 targetPos = targetTask.position;
        NavMeshHit hitEnd;
        if (NavMesh.SamplePosition(targetTask.position, out hitEnd, navMeshSampleRadius, NavMesh.AllAreas))
            targetPos = hitEnd.position;

        bool pathFound = NavMesh.CalculatePath(startPos, targetPos, NavMesh.AllAreas, path);
        if (!pathFound || path.corners.Length < 2)
        {
            BuildOptimizedLine(new Vector3[] { startPos, targetPos + Vector3.up * heightAboveGround });
        }
        else
        {
            BuildOptimizedLine(path.corners);
        }

        // --- Animación de textura ---
        // Solo tiempo
        cumulativeOffset += Time.deltaTime * textureScrollSpeed;

        // Añadimos un pequeño componente proporcional al movimiento del jugador
        Vector3 playerDelta = playerTransform.position - lastPlayerPos;
        if (lineRenderer.positionCount >= 2)
        {
            Vector3 pathDir = (lineRenderer.GetPosition(lineRenderer.positionCount - 1) - lineRenderer.GetPosition(0)).normalized;
            float playerAlongPath = Vector3.Dot(playerDelta, pathDir);
            cumulativeOffset += playerAlongPath * playerOffsetFactor;
        }

        lineMat.SetTextureOffset("_BaseMap", new Vector2(-cumulativeOffset / tileSizeInWorld, 0));

        lastPlayerPos = playerTransform.position;
    }

    private void BuildOptimizedLine(Vector3[] corners)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < corners.Length - 1; i++)
        {
            Vector3 start = corners[i] + Vector3.up * heightAboveGround;
            Vector3 end = corners[i + 1] + Vector3.up * heightAboveGround;

            float dist = Vector3.Distance(start, end);
            int segments = Mathf.Max(1, Mathf.CeilToInt(dist / segmentLength));

            for (int s = 0; s < segments; s++)
            {
                float t = (float)s / segments;
                points.Add(Vector3.Lerp(start, end, t));
            }
        }

        points.Add(corners[corners.Length - 1] + Vector3.up * heightAboveGround);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private Transform GetNextActiveTask()
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 playerPos = playerTransform.position;

        foreach (GameObject tareaObj in tareas.OrdenTareas)
        {
            if (tareaObj == null || !tareaObj.activeInHierarchy) continue;

            float dist = Vector3.Distance(playerPos, tareaObj.transform.position);

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(tareaObj.transform.position, out hit, navMeshSampleRadius, NavMesh.AllAreas))
                continue;

            NavMeshPath testPath = new NavMeshPath();
            if (!NavMesh.CalculatePath(playerTransform.position, hit.position, NavMesh.AllAreas, testPath))
                continue;

            if (testPath.status != NavMeshPathStatus.PathComplete &&
                testPath.status != NavMeshPathStatus.PathPartial)
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
