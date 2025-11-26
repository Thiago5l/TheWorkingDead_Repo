using UnityEngine;

public class BzzOrbit : MonoBehaviour
{
    public Transform target;

    [Header("Orbit Settings")]
    public float orbitRadius = 2f;
    public float orbitSpeed = 3f;

    [Header("Flight Noise")]
    public float noiseAmount = 0.4f;
    public float noiseSpeed = 2f;

    [Header("Height")]
    public float heightOffset = 1.5f;

    [Header("Follow Smoothing")]
    public float smoothTime = 0.2f;
    public float overshootAmount = 0.15f;

    float angle;
    Vector3 velocity;

    void Update()
    {
        if (target == null) return;

        angle += orbitSpeed * Time.deltaTime;

        float x = Mathf.Cos(angle) * orbitRadius;
        float z = Mathf.Sin(angle) * orbitRadius;

        float nx = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0f) - 0.5f) * noiseAmount;
        float ny = (Mathf.PerlinNoise(0f, Time.time * noiseSpeed) - 0.5f) * noiseAmount;
        float nz = (Mathf.PerlinNoise(Time.time * noiseSpeed, Time.time * 0.5f) - 0.5f) * noiseAmount;

        Vector3 rawTargetPos = target.position + new Vector3(x + nx, heightOffset + ny, z + nz);

        // Añadir overshoot (pasarse un poco del objetivo)
        Vector3 offsetDir = (rawTargetPos - transform.position).normalized;
        rawTargetPos += offsetDir * overshootAmount;

        // Suavizado con inercia
        transform.position = Vector3.SmoothDamp(transform.position, rawTargetPos, ref velocity, smoothTime);

        // Rotación hacia donde va
        Vector3 dir = velocity.normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
