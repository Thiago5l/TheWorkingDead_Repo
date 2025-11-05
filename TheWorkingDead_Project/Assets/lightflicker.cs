using UnityEngine;

// LightFlicker.cs
using System.Collections;

[DisallowMultipleComponent]
public class LightFlicker : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Si no se asigna, intentará obtener un Light en el mismo GameObject.")]
    public Light targetLight;

    [Tooltip("Opcional: renderer cuyo material tenga emisión (para 'ver' el parpadeo).")]
    public Renderer targetRenderer;

    [Header("Comportamiento")]
    [Tooltip("Si true, los intervalos y la intensidad serán aleatorios entre min/max.")]
    public bool randomize = true;

    [Tooltip("Intensidad mínima de la luz.")]
    public float minIntensity = 0f;
    [Tooltip("Intensidad máxima de la luz.")]
    public float maxIntensity = 1.5f;

    [Tooltip("Tiempo mínimo entre cambios (en segundos).")]
    public float minInterval = 0.02f;
    [Tooltip("Tiempo máximo entre cambios (en segundos).")]
    public float maxInterval = 0.3f;

    [Header("Opciones avanzadas")]
    [Tooltip("Si true, usará un pulso suave entre intensidades en lugar de cambios instantáneos.")]
    public bool smoothTransitions = false;
    [Tooltip("Duración del pulso suave (si smoothTransitions = true).")]
    public float smoothDuration = 0.08f;

    [Tooltip("Color base de la emisión (se multiplica por la intensidad).")]
    public Color emissionColor = Color.white;

    // Para no tocar sharedMaterial accidentalmente
    Material runtimeMaterial;

    void Awake()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        if (targetRenderer != null)
        {
            // crea una instancia del material para no alterar sharedMaterial globalmente
            runtimeMaterial = targetRenderer.material;
            // asegúrate de que la keyword de emisión esté activada
            runtimeMaterial.EnableKeyword("_EMISSION");
        }
    }

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(FlickerLoop());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            float nextInterval = randomize ? Random.Range(minInterval, maxInterval) : minInterval;
            float targetIntensity = randomize ? Random.Range(minIntensity, maxIntensity) : maxIntensity;

            if (smoothTransitions)
            {
                // transición suave desde la intensidad actual hasta la targetIntensity
                float startIntensity = (targetLight != null) ? targetLight.intensity : GetCurrentEmissionIntensity();
                float t = 0f;
                while (t < smoothDuration)
                {
                    t += Time.deltaTime;
                    float lerp = Mathf.Clamp01(t / smoothDuration);
                    float curIntensity = Mathf.Lerp(startIntensity, targetIntensity, lerp);
                    ApplyIntensity(curIntensity);
                    yield return null;
                }
            }
            else
            {
                ApplyIntensity(targetIntensity);
            }

            yield return new WaitForSeconds(nextInterval);
        }
    }

    void ApplyIntensity(float intensity)
    {
        if (targetLight != null)
            targetLight.intensity = intensity;

        if (runtimeMaterial != null)
        {
            // Emission color = baseColor * intensity (HDR)
            runtimeMaterial.SetColor("_EmissionColor", emissionColor * intensity);
            // En algunos casos (Play/Pause en editor) puede ser necesario forzar actualización:
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(runtimeMaterial);
#endif
        }
    }

    float GetCurrentEmissionIntensity()
    {
        if (runtimeMaterial != null)
        {
            Color c = runtimeMaterial.GetColor("_EmissionColor");
            // aproximación: intensidad = brillo promedio de RGB
            return (c.r + c.g + c.b) / 3f;
        }
        if (targetLight != null) return targetLight.intensity;
        return 0f;
    }

    // Método público para disparar un "golpe" de intensidad desde código
    public void Pulse(float intensity, float duration)
    {
        StopCoroutine("PulseCoroutine");
        StartCoroutine(PulseCoroutine(intensity, duration));
    }

    IEnumerator PulseCoroutine(float intensity, float duration)
    {
        float original = (targetLight != null) ? targetLight.intensity : GetCurrentEmissionIntensity();
        // sube rápido
        ApplyIntensity(intensity);
        yield return new WaitForSeconds(duration);
        // vuelve a original
        ApplyIntensity(original);
    }
}


