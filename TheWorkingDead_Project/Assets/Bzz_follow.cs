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

    float angle;

    void Update()
    {
        if (target == null) return;

        // Orbit angle increase over time
        angle += orbitSpeed * Time.deltaTime;

        // Circle position around target in XZ plane
        float x = Mathf.Cos(angle) * orbitRadius;
        float z = Mathf.Sin(angle) * orbitRadius;

        // Noise movement (random buzzing)
        float nx = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0f) - 0.5f) * noiseAmount;
        float ny = (Mathf.PerlinNoise(0f, Time.time * noiseSpeed) - 0.5f) * noiseAmount;
        float nz = (Mathf.PerlinNoise(Time.time * noiseSpeed, Time.time * 0.5f) - 0.5f) * noiseAmount;

        // Final position
        Vector3 orbitPos = target.position + new Vector3(x + nx, heightOffset + ny, z + nz);

        transform.position = orbitPos;

        // Look where it's going
        Vector3 dir = (orbitPos - transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
