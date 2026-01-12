using UnityEngine;
using DG.Tweening;

public class ShowWithTab : MonoBehaviour
{
    public GameObject panel;
    public CanvasGroup canvasGroup;

    public float duration = 0.25f;
    public float offsetX = -500f; // Distancia horizontal desde la que aparece el panel

    private Tween currentTween;
    private bool isOpen;

    private Vector3 originalPosition;

    void Start()
    {
        // Guardamos la posición original del panel
        originalPosition = panel.transform.localPosition;

        // Inicializamos el panel oculto
        panel.SetActive(false);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isOpen)
        {
            Open();
        }

        if (Input.GetKeyUp(KeyCode.Tab) && isOpen)
        {
            Close();
        }
    }

    void Open()
    {
        isOpen = true;
        panel.SetActive(true);

        currentTween?.Kill();

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Movemos el panel fuera de la pantalla horizontalmente
        panel.transform.localPosition = originalPosition + new Vector3(offsetX, 0f, 0f);
        canvasGroup.alpha = 0f;

        if (duration <= 0f || Time.timeScale == 0f)
        {
            // Aplicar instantáneamente si duration es 0 o el juego pausado
            canvasGroup.alpha = 1f;
            panel.transform.localPosition = originalPosition;
        }
        else
        {
            currentTween = DOTween.Sequence()
                .Append(canvasGroup.DOFade(1f, duration).SetUpdate(true))
                .Join(panel.transform.DOLocalMove(originalPosition, duration).SetEase(Ease.OutBack).SetUpdate(true));
        }
    }

    void Close()
    {
        isOpen = false;

        currentTween?.Kill();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (duration <= 0f || Time.timeScale == 0f)
        {
            canvasGroup.alpha = 0f;
            panel.transform.localPosition = originalPosition + new Vector3(offsetX, 0f, 0f);
            panel.SetActive(false);
        }
        else
        {
            currentTween = DOTween.Sequence()
                .Append(canvasGroup.DOFade(0f, duration).SetUpdate(true))
                .Join(panel.transform.DOLocalMove(originalPosition + new Vector3(offsetX, 0f, 0f), duration).SetEase(Ease.InBack).SetUpdate(true))
                .OnComplete(() =>
                {
                    panel.SetActive(false);
                });
        }
    }
}
