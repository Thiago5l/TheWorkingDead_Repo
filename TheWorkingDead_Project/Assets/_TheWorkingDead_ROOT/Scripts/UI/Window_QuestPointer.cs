using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_QuestPointer : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera uiCamera;        // Cámara de la UI
    [SerializeField] private Transform playerTransform; // Posición desde donde buscar tasks
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;

    [Header("Configuración")]
    [SerializeField] private float detectionRadius = 100f; // Radio de detección de tasks
    [SerializeField] private LayerMask taskLayer;         // Layer de tasks
    [SerializeField] private float borderSize = 100f;     // Margen de pantalla para mostrar la flecha

    private Vector3 targetPosition;
    private RectTransform pointerRectTransform;
    private Image pointerImage;

    private void Awake()
    {
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerImage = transform.Find("Pointer").GetComponent<Image>();
    }

    private void Update()
    {
        // 1. Obtener la tarea más cercana al jugador
        Transform closestTask = GetClosestTask();
        if (closestTask != null)
        {
            targetPosition = closestTask.position;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            return;
        }

        // 2. Convertir la posición del target a coordenadas de pantalla
        Vector3 targetScreenPos = uiCamera.WorldToScreenPoint(targetPosition);

        // 3. Verificar si el target está fuera de pantalla o detrás de la cámara
        bool isOffScreen = targetScreenPos.x <= borderSize || targetScreenPos.x >= Screen.width - borderSize
                         || targetScreenPos.y <= borderSize || targetScreenPos.y >= Screen.height - borderSize
                         || targetScreenPos.z < 0;

        if (isOffScreen)
        {
            // Flecha apuntando hacia el target
            pointerImage.sprite = arrowSprite;
            RotatePointerTowardsTargetPosition();

            // Limitar posición del puntero dentro de la pantalla
            Vector3 cappedPos = targetScreenPos;
            cappedPos.x = Mathf.Clamp(cappedPos.x, borderSize, Screen.width - borderSize);
            cappedPos.y = Mathf.Clamp(cappedPos.y, borderSize, Screen.height - borderSize);

            Vector3 pointerWorldPos = uiCamera.ScreenToWorldPoint(cappedPos);
            pointerRectTransform.position = pointerWorldPos;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        }
        else
        {
            // Cruz indicando que el target está dentro de la pantalla
            pointerImage.sprite = crossSprite;
            pointerRectTransform.localEulerAngles = Vector3.zero;

            Vector3 pointerWorldPos = uiCamera.ScreenToWorldPoint(targetScreenPos);
            pointerRectTransform.position = pointerWorldPos;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        }
    }

    private void RotatePointerTowardsTargetPosition()
    {
        Vector3 playerScreenPos = uiCamera.WorldToScreenPoint(playerTransform.position);
        Vector3 targetScreenPos = uiCamera.WorldToScreenPoint(targetPosition);

        Vector3 dir = (targetScreenPos - playerScreenPos).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public Transform GetClosestTask()
    {
        Collider[] hits = Physics.OverlapSphere(playerTransform.position, detectionRadius, taskLayer);

        Transform closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = playerTransform.position;

        foreach (Collider col in hits)
        {
            float dist = Vector3.Distance(currentPos, col.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = col.transform;
            }
        }

        return closest;
    }

    public void Show(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        gameObject.SetActive(true);
    }
}
