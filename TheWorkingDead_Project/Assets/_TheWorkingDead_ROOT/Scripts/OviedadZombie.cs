using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OviedadZombie : MonoBehaviour
{



    [Header("Values")]
    [SerializeField] public float Zombiedad;
    [SerializeField] public float maxZombiedad = 100;
    [SerializeField] public float ZombiedadSpeed = 100;
    [SerializeField] public float Zombiedadocupadomultiplier = 0.25f;
    private float zombiedadSpeedOriginal;
    [Header("Game Objects")]
    [SerializeField] Slider zombiedadBar;
    [SerializeField] GameObject looseCanvas;
    [SerializeField] PlayerController playerController;
    [Header("Sprites")]
    
    [SerializeField] Image gregHeadImage;
    [SerializeField] Sprite[] gregSprites;

    [Header("Box volume")]
    [SerializeField] Volume volume;
    ChromaticAberration chromaticAberration;
    Vignette vignette;
    Coroutine mediumEffectCoroutine;

    [SerializeField] float mediumChromaticTarget = 0.3f;
    [SerializeField] float mediumVignetteTarget = 0.4f;
    [SerializeField] float transitionDuration = 0.5f;

    bool playshake;
    bool playhighshake;
    RectTransform zombiedadBarRect;
    Vector2 originalBarPos;


    //[Header("For Sprite Movement")]
    //[SerializeField] Transform gregHeadTransform;
    //[SerializeField] Vector2 endPosition = new Vector2(0, 0);
    //[SerializeField] Vector2 startPosition;
    //[SerializeField] float desiredDuration = 3f;
    //[SerializeField] float elapsedTime;

    //[Header("Others")]
    //[SerializeField] Transform PlayerTransform;
    //[SerializeField] Transform RespawnPoint;


    void Start()
    {

        zombiedadSpeedOriginal= ZombiedadSpeed;
        Zombiedad = maxZombiedad;
        playerController = this.gameObject.GetComponent<PlayerController>();

        zombiedadBar.maxValue = maxZombiedad;
        zombiedadBar.value = Zombiedad;

        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out vignette);

        if (vignette != null)
        {
            vignette.intensity.overrideState = true;
        }
        zombiedadBarRect = zombiedadBar.GetComponent<RectTransform>();
        originalBarPos = zombiedadBarRect.anchoredPosition;

        //

        //startPosition = gregHeadTransform.localPosition;
    }
    //-----//
    private void Update()
    {
        // Limitar Zombiedad
        Zombiedad = Mathf.Clamp(Zombiedad - ZombiedadSpeed * Time.deltaTime, 0f, 100f);

        // Actualizar slider suavemente
        zombiedadBar.value = Mathf.Lerp(zombiedadBar.value, Zombiedad, Time.deltaTime * 5f);

        // Revisar si perdió
        if (Zombiedad <= 0f)
        {
            looseCanvas.SetActive(true);
            playerController.playerOcupado = true;
            return; // nada más que hacer
        }

        // Estados de zombiedad

        if (Zombiedad >= 50f)
        {
            gregHeadImage.sprite = gregSprites[0];
            chromaticAberration.intensity.value = 0f;
            vignette.intensity.value = 0.3f;

                StopShakeZombiedadBar();
        }
        else if (Zombiedad >= 25f)
        {
            gregHeadImage.sprite = gregSprites[1];

            if (mediumEffectCoroutine == null)
                mediumEffectCoroutine = StartCoroutine(MediumEffectTransition());
                ShakeZombiedadBar();
        }
        else
        {
            gregHeadImage.sprite = gregSprites[2];
            chromaticAberration.intensity.value = 0.4f + Mathf.PingPong(Time.time * 3f, 0.3f);
            vignette.intensity.value = 0.4f + Mathf.PingPong(Time.time * 0.5f, 0.1f);

                ShakeZombiedadBarHigh();
        }


    }

    public void resetspeed()
    { ZombiedadSpeed = zombiedadSpeedOriginal; }
    IEnumerator MediumEffectTransition()
    {
        float startChromatic = chromaticAberration.intensity.value;
        float startVignette = vignette.intensity.value;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;

            chromaticAberration.intensity.value =
                Mathf.Lerp(startChromatic, mediumChromaticTarget, t);

            vignette.intensity.value =
                Mathf.Lerp(startVignette, mediumVignetteTarget, t);

            yield return null;
        }

        chromaticAberration.intensity.value = mediumChromaticTarget;
        vignette.intensity.value = mediumVignetteTarget;
    }
    Tween shakeTween;

    void ShakeZombiedadBar()
    {
        StopShakeZombiedadBar(); // detiene cualquier shake previo

        shakeTween = zombiedadBarRect.DOShakeAnchorPos(
            1f,   // duración de un ciclo
            2f,   // fuerza
            5,    // vibraciones
            90f,  // randomness
            false,
            true
        )
        .SetLoops(-1, LoopType.Yoyo) // Yoyo para volver siempre al original
        .OnKill(() => {
            // Restaurar posición original al detener
            zombiedadBarRect.anchoredPosition = originalBarPos;
        });
    }


    void ShakeZombiedadBarHigh()
    {
        StopShakeZombiedadBar();

        shakeTween = zombiedadBarRect.DOShakeAnchorPos(
            1f,
            4f,
            10,
            90f,
            false,
            true
        )
        .SetLoops(-1, LoopType.Yoyo)
        .OnKill(() => {
            zombiedadBarRect.anchoredPosition = originalBarPos;
        });
    }



    void StopShakeZombiedadBar()
    {
        if (shakeTween != null)
        {
            shakeTween.Kill();
            shakeTween = null;
            // El OnKill() restaurará la posición automáticamente
        }
    }







    //    #region General Variables
    //    public float Zombiedad = 0;
    //    [SerializeField] Slider ZombiedadBar;
    //    [SerializeField] Transform PlayerTransform;
    //    [SerializeField] Transform RespawnPoint;
    //    [SerializeField] public float sumValue;
    //    [SerializeField] float time;
    //    [SerializeField] public GameObject LooseCanvas;
    //    #endregion

    //    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //    void Start()
    //    {
    //        if (ZombiedadBar == null)
    //        {
    //            // Busca el objeto "Fill" en la jerarquía
    //            ZombiedadBar = GameObject.Find("Fill").GetComponent<Slider>();
    //        }
    //        float zValue = 1;
    //        StartCoroutine(WaitObviedad(zValue));
    //    }
    //   IEnumerator WaitObviedad(float duration)
    //    {
    //         while (Zombiedad< 1)
    //         {
    //            yield return new WaitForSeconds(time);
    //            Zombiedad = Zombiedad + sumValue;          
    //         }

    //    } 




    //// Update is called once per frame

    //private void Update()
    //{
    //        ZombiedadBar.GetComponent<Image>().fillAmount = Zombiedad;

    //        if (Zombiedad < 0)
    //        {
    //            Zombiedad = 0;
    //        }

    //        if (Zombiedad >= 1)
    //        {
    //            LooseCanvas.SetActive(true);
    //        }
    //}
}
