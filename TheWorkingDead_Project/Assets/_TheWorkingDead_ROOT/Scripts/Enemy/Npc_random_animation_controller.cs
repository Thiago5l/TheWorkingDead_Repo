using UnityEngine;

public class NpcRandomScratch : MonoBehaviour
{
    public Animator animator;

    [Header("Tiempo entre animaciones")]
    public float minDelay = 10f;
    public float maxDelay = 12f;

    [Header("Nombre del parámetro (ej.: 'rascar' o 'rascarmano')")]
    public string paramName = "rascar";

    private float timer;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            StartCoroutine(ActivateParam());
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(minDelay, maxDelay);
    }

    private System.Collections.IEnumerator ActivateParam()
    {
        animator.SetFloat(paramName, 1f);   // lo activas
        yield return null;                  // 1 frame (puedes cambiarlo)
        animator.SetFloat(paramName, 0f);   // lo vuelves a 0
    }
}
