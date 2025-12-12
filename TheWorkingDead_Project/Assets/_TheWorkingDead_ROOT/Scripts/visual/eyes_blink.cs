using UnityEngine;
using System.Collections;

public class eyes_blink : MonoBehaviour
{
    [Header("Renderer del ojo")]
    public Renderer eyeRenderer;

    [Header("Materiales en orden: abierto, medio, cerrado")]
    public Material eyeOpen;
    public Material eyeHalf;
    public Material eyeClosed;

    [Header("Frecuencia del parpadeo")]
    public float minBlinkDelay = 3f;  // segundos mínimo entre parpadeos
    public float maxBlinkDelay = 7f;  // segundos máximo entre parpadeos

    [Header("Velocidad del parpadeo")]
    public float blinkSpeed = 0.08f; // segundos entre estados

    private void Start()
    {
        eyeRenderer.material = eyeOpen;
        if (eyeRenderer == null)
            eyeRenderer = GetComponent<Renderer>();

        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // Espera un tiempo aleatorio entre parpadeos
            float wait = Random.Range(minBlinkDelay, maxBlinkDelay);
            yield return new WaitForSeconds(wait);

            // Secuencia de parpadeo
            if (eyeRenderer && eyeOpen && eyeHalf && eyeClosed)
            {
                // medio
                eyeRenderer.material = eyeHalf;
                yield return new WaitForSeconds(blinkSpeed);

                // cerrado
                eyeRenderer.material = eyeClosed;
                yield return new WaitForSeconds(blinkSpeed);

                // medio
                eyeRenderer.material = eyeHalf;
                yield return new WaitForSeconds(blinkSpeed);

                // abierto
                eyeRenderer.material = eyeOpen;
            }
        }
    }
}
