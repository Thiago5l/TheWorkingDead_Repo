using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeCanvas : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup canvasGroup;
    public Image fadeImage;
    [SerializeField] GameObject Player;

    [Header("Configuracion Fade")]
    public float fadeDuration = 1f;
    [Range(0f, 1f)] public float maxAlpha = 0.4f;
    public Color winColor = Color.green;
    public Color loseColor = Color.red;

    [Header("Gameplay")]
    public float penalizacion;
    public float premio;

    [Header("Bloqueo inicial")]
    [SerializeField] private float fadeLockTime = 2f;

    private Coroutine currentFade;
    private bool fadeEnabled = false;

    private void Awake()
    {
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        if (!fadeImage) fadeImage = GetComponent<Image>();

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    private void Start()
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(EnableFadeAfterDelay());
    }

    private IEnumerator EnableFadeAfterDelay()
    {
        yield return new WaitForSeconds(fadeLockTime);
        fadeEnabled = true;
    }

    public void PlayWin()
    {
        Player.GetComponent<OviedadZombie>().Zombiedad += premio;
        StartFade(winColor);
    }

    public void PlayLose()
    {
        Player.GetComponent<OviedadZombie>().Zombiedad -= penalizacion;
        StartFade(loseColor);
    }

    private void StartFade(Color color)
    {
        if (!fadeEnabled) return;

        if (currentFade != null)
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(Fade(color));
    }

    private IEnumerator Fade(Color color)
    {
        fadeImage.color = color;
        canvasGroup.blocksRaycasts = true;

        float t = 0f;

        // Fade In
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, maxAlpha, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = maxAlpha;
        t = 0f;

        // Fade Out
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(maxAlpha, 0f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        currentFade = null;
    }
}
