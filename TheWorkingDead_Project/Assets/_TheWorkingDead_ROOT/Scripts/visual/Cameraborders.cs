using UnityEngine;

public class CameraFollowWithLimits : MonoBehaviour
{
    public Transform player; // El jugador o el objeto a seguir
    public Vector3 offset;   // La distancia de la cámara respecto al jugador

    // Límites del mapa
    public Vector3 minLimit;
    public Vector3 maxLimit;

    void LateUpdate()
    {
        if (player == null) return;

        // Posición deseada de la cámara respecto al jugador
        Vector3 desiredPos = player.position + offset;

        // Limitar la posición de la cámara
        desiredPos.x = Mathf.Clamp(desiredPos.x, minLimit.x, maxLimit.x);
        desiredPos.y = Mathf.Clamp(desiredPos.y, minLimit.y, maxLimit.y);
        desiredPos.z = Mathf.Clamp(desiredPos.z, minLimit.z, maxLimit.z);

        // Asignar la posición limitada
        transform.position = desiredPos;
    }
}
