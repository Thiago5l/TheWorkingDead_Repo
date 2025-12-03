using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool automaticallyFadeInOnSceneLoad;
    private Coroutine fadeCoroutine;
    [SerializeField] private float automaticFadeStartAlpha;
    [SerializeField] private float automaticFadeEndAlpha;
    [SerializeField] private float automaticFadeDuration;
    [SerializeField] private float automaticFadeDelayBeforeFade;
    private void Awake()
    {
        
        if (automaticallyFadeInOnSceneLoad)
        {
            DoFade(automaticFadeStartAlpha, automaticFadeEndAlpha, automaticFadeDuration, automaticFadeDelayBeforeFade);
        }
        
    }
    public void DoFade(float starAlpha, float endAlpha, float duration, float delayBeforeFade)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(AnimateFade(starAlpha, endAlpha, duration, delayBeforeFade));
    }
    private IEnumerator AnimateFade(float starAlpha, float endAlpha, float duration, float delayBeforeFade)
    {
        fadeImage.enabled = true;
        canvasGroup.alpha = starAlpha;
        yield return null;
        yield return new WaitForSecondsRealtime(delayBeforeFade);
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.unscaledDeltaTime;
            float fadePercentage = timeElapsed / duration;
            fadePercentage = Mathf.Clamp01(fadePercentage);
            canvasGroup.alpha = Mathf.Lerp(starAlpha,endAlpha,fadePercentage);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
        if (endAlpha <= 0)
        {
            fadeImage.enabled = false;
        }
    }
}
