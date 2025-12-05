using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class HoldToSkipVideo : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer videoPlayer;

    [Header("Skip Settings")]
    public KeyCode skipKey = KeyCode.Space;
    public float holdDuration = 1.5f;

    [Header("UI Elements")]
    public Image skipFillImage;
    public CanvasGroup startTextGroup; // Texto que aparece al inicio y al presionar espacio

    [Header("Start Text Timing")]
    public float fadeInDuration = 0.8f;
    public float startTextDuration = 2f;
    public float fadeOutDuration = 1f;

    [Header("Hold Press Text Fade")]
    public float holdTextFadeDuration = 0.3f; // fade rápido al presionar espacio

    private float holdTimer = 0f;
    private bool startFadeFinished = false;
    private Coroutine holdTextCoroutine;

    void Start()
    {
        if (skipFillImage != null)
            skipFillImage.fillAmount = 0f;

        if (startTextGroup != null)
            startTextGroup.alpha = 0f; // inicia invisible

        if (startTextGroup != null)
            StartCoroutine(StartTextSequence());
        else
            startFadeFinished = true; // si no hay texto, skip disponible desde el inicio
    }

    IEnumerator StartTextSequence()
    {
        // FADE IN inicial
        float tIn = 0f;
        while (tIn < fadeInDuration)
        {
            tIn += Time.deltaTime;
            if (startTextGroup != null)
                startTextGroup.alpha = Mathf.Clamp01(tIn / fadeInDuration);
            yield return null;
        }

        if (startTextGroup != null)
            startTextGroup.alpha = 1f;

        yield return new WaitForSeconds(startTextDuration);

        // FADE OUT inicial
        float tOut = 0f;
        while (tOut < fadeOutDuration)
        {
            tOut += Time.deltaTime;
            if (startTextGroup != null)
                startTextGroup.alpha = 1f - Mathf.Clamp01(tOut / fadeOutDuration);
            yield return null;
        }

        if (startTextGroup != null)
            startTextGroup.alpha = 0f;

        startFadeFinished = true;
    }

    void Update()
    {
        if (videoPlayer == null || !videoPlayer.isPlaying)
            return;

        if (!startFadeFinished)
            return;

        // HOLD-TO-SKIP
        if (Input.GetKey(skipKey))
        {
            // Mostrar rápidamente el texto mientras mantienes presionado
            if (holdTextCoroutine != null) StopCoroutine(holdTextCoroutine);
            holdTextCoroutine = StartCoroutine(FadeText(startTextGroup, 1f, holdTextFadeDuration));

            holdTimer += Time.deltaTime;

            if (skipFillImage != null)
                skipFillImage.fillAmount = Mathf.Clamp01(holdTimer / holdDuration);

            if (holdTimer >= holdDuration)
                SkipVideo();
        }
        else
        {
            // Reiniciar
            holdTimer = 0f;
            if (skipFillImage != null)
                skipFillImage.fillAmount = 0f;

            // Fade-out rápido del texto cuando sueltas la tecla
            if (holdTextCoroutine != null) StopCoroutine(holdTextCoroutine);
            holdTextCoroutine = StartCoroutine(FadeText(startTextGroup, 0f, holdTextFadeDuration));
        }
    }

    IEnumerator FadeText(CanvasGroup cg, float targetAlpha, float duration)
    {
        if (cg == null) yield break;

        float startAlpha = cg.alpha;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, t / duration);
            yield return null;
        }

        cg.alpha = targetAlpha;
    }

    void SkipVideo()
    {
        if (videoPlayer != null && videoPlayer.frameCount > 0)
        {
            long lastFrame = (long)videoPlayer.frameCount - 1;
            videoPlayer.frame = lastFrame;
            videoPlayer.Stop();
        }

        SceneManager.LoadScene("SCN_Office_Level1");
    }
}
