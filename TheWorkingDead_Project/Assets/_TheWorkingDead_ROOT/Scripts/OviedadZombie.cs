using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OviedadZombie : MonoBehaviour
{
    [Header("Values")]
    public float Zombiedad;
    public float ZombiedadSpeed;

    [SerializeField] float maxZombiedad = 100f;
    [SerializeField] float zombiedadOcupadoSpeed = 0.5f;

    float zombiedadSpeedOriginal;

    [Header("Game Objects")]
    [SerializeField] Slider zombiedadBar;
    [SerializeField] GameObject looseCanvas;
    [SerializeField] PlayerController playerController;

    [Header("Sprites")]
    [SerializeField] Image gregHeadImage;
    [SerializeField] Sprite[] gregSprites;

    [Header("Post Process")]
    [SerializeField] Volume volume;
    ChromaticAberration chromaticAberration;
    Vignette vignette;
    Coroutine mediumEffectCoroutine;

    [Header("Effects")]
    [SerializeField] float mediumChromaticTarget = 0.3f;
    [SerializeField] float mediumVignetteTarget = 0.4f;
    [SerializeField] float transitionDuration = 0.5f;

    [Header("UI Shake")]
    RectTransform zombiedadBarRect;
    Vector2 originalBarPos;
    Tween shakeTween;
    bool isShaking;
    bool isHighShaking;

    public bool snackActivo;

    void Awake()
    {
        DOTween.Init();
    }

    void Start()
    {
        zombiedadSpeedOriginal = ZombiedadSpeed;
        Zombiedad = maxZombiedad;

        zombiedadBar.maxValue = maxZombiedad;
        zombiedadBar.value = Zombiedad;


        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out vignette);
        vignette.intensity.overrideState = true;

        zombiedadBarRect = zombiedadBar.GetComponent<RectTransform>();
        originalBarPos = zombiedadBarRect.anchoredPosition;
    }

    void Update()
    {
        HandleSpeed();
        UpdateZombiedad();
        UpdateUI();
    }

    void HandleSpeed()
    {
        if (playerController.playerOcupado)
            ZombiedadSpeed = zombiedadOcupadoSpeed;
        else if (!snackActivo)
            resetspeed();

    }

    void UpdateZombiedad()
    {
        Zombiedad = Mathf.Clamp(
           Zombiedad - ZombiedadSpeed * Time.deltaTime,
           0f,
           maxZombiedad
       );

        zombiedadBar.value = Mathf.Lerp(
            zombiedadBar.value,
            Zombiedad,
            Time.deltaTime * 5f
        );

        if (Zombiedad <= 0f)
        {
            looseCanvas.SetActive(true);
            playerController.playerOcupado = true;
        }

    }

    void UpdateUI()
    {
        if (Zombiedad >= 50f)
        {
            gregHeadImage.sprite = gregSprites[0];
            chromaticAberration.intensity.value = 0f;
            vignette.intensity.value = 0.3f;
            StopShake();
        }
        else if (Zombiedad >= 25f)
        {
            gregHeadImage.sprite = gregSprites[1];

            if (mediumEffectCoroutine == null)
                mediumEffectCoroutine = StartCoroutine(MediumEffectTransition());

            if (!isShaking)
                StartShake(2f, 5, false);
        }
        else
        {
            gregHeadImage.sprite = gregSprites[2];
            chromaticAberration.intensity.value =
                0.4f + Mathf.PingPong(Time.time * 3f, 0.3f);
            vignette.intensity.value =
                0.4f + Mathf.PingPong(Time.time * 0.5f, 0.1f);

            if (!isHighShaking)
                StartShake(4f, 10, true);
        }
    }
    public void resetspeed()
    {
        ZombiedadSpeed = zombiedadSpeedOriginal;
    }
    IEnumerator MediumEffectTransition()
    {
        float startC = chromaticAberration.intensity.value;
        float startV = vignette.intensity.value;
        float t = 0f;

        while (t < transitionDuration)
        {
            t += Time.deltaTime;
            float lerp = t / transitionDuration;

            chromaticAberration.intensity.value =
                Mathf.Lerp(startC, mediumChromaticTarget, lerp);
            vignette.intensity.value =
                Mathf.Lerp(startV, mediumVignetteTarget, lerp);

            yield return null;
        }
    }

    void StartShake(float strength, int vibrato, bool high)
    {
        StopShake();

        isShaking = !high;
        isHighShaking = high;

        shakeTween = zombiedadBarRect
            .DOShakeAnchorPos(1f, strength, vibrato, 90f, false, true)
            .SetLoops(-1, LoopType.Yoyo)
            .OnKill(() =>
            {
                zombiedadBarRect.anchoredPosition = originalBarPos;
                isShaking = false;
                isHighShaking = false;
            });
    }

    void StopShake()
    {
        if (shakeTween != null)
        {
            shakeTween.Kill();
            shakeTween = null;
        }
    }
}
