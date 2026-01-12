using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_QuestPointer : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private TareasAleatorias tareas;   // << AÑADIDO
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;

    [Header("Configuración")]
    [SerializeField] private float borderSize = 100f;

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
        // 1. Obtener la tarea más cercana desde la lista
        Transform closestTask = GetClosestTaskFromList();
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

        Vector3 targetScreenPos = uiCamera.WorldToScreenPoint(targetPosition);

        bool isOffScreen =
            targetScreenPos.x <= borderSize || targetScreenPos.x >= Screen.width - borderSize ||
            targetScreenPos.y <= borderSize || targetScreenPos.y >= Screen.height - borderSize ||
            targetScreenPos.z < 0;

        if (isOffScreen)
        {
            pointerImage.sprite = arrowSprite;
            RotatePointerTowardsTargetPosition();

            Vector3 cappedPos = targetScreenPos;
            cappedPos.x = Mathf.Clamp(cappedPos.x, borderSize, Screen.width - borderSize);
            cappedPos.y = Mathf.Clamp(cappedPos.y, borderSize, Screen.height - borderSize);

            Vector3 pointerWorldPos = uiCamera.ScreenToWorldPoint(cappedPos);
            pointerRectTransform.position = pointerWorldPos;

            pointerRectTransform.localPosition =
                new Vector3(pointerRectTransform.localPosition.x,
                            pointerRectTransform.localPosition.y,
                            0f);
        }
        else
        {
            pointerImage.sprite = crossSprite;
            pointerRectTransform.localEulerAngles = Vector3.zero;

            Vector3 pointerWorldPos = uiCamera.ScreenToWorldPoint(targetScreenPos);
            pointerRectTransform.position = pointerWorldPos;

            pointerRectTransform.localPosition =
                new Vector3(pointerRectTransform.localPosition.x,
                            pointerRectTransform.localPosition.y,
                            0f);
        }
    }

    // 🔥 Nueva versión: obtiene la tarea más cercana desde tareas.OrdenTareas
    private Transform GetClosestTaskFromList()
    {
        if (tareas == null || tareas.OrdenTareas.Count == 0)
            return null;

        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 playerPos = playerTransform.position;

        foreach (GameObject tareaObj in tareas.OrdenTareas)
        {
            if (tareaObj == null) continue;

            Transform taskTransform = tareaObj.transform;

            float dist = Vector3.Distance(playerPos, taskTransform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = taskTransform;
            }
        }
        if (tareas == null)
        {
            Debug.LogError("NO HAY REFERENCIA AL SCRIPT TareasAleatorias");
            return null;
        }

        if (tareas.OrdenTareas.Count == 0)
        {
            Debug.LogWarning("OrdenTareas ESTA VACIA");
            return null;
        }

        foreach (var t in tareas.OrdenTareas)
        {
            if (t == null) Debug.Log("Tarea NULL en la lista");
        }
        return closest;
    }

    private void RotatePointerTowardsTargetPosition()
    {
        Vector3 playerScreenPos = uiCamera.WorldToScreenPoint(playerTransform.position);
        Vector3 targetScreenPos = uiCamera.WorldToScreenPoint(targetPosition);

        Vector3 dir = (targetScreenPos - playerScreenPos).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
