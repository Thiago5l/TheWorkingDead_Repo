using UnityEngine;
using System.Collections;

public class AireAcondicionadoWinCanvas : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] float fadeInDuration = 0.3f;
    [SerializeField] float delay = 1f;
    [SerializeField] float fadeOutDuration = 0.5f;

    Coroutine fadeRoutine;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // FADE IN
        canvasGroup.alpha = 0f;
        float t = 0f;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // ESPERA
        yield return new WaitForSeconds(delay);

        // FADE OUT
        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
