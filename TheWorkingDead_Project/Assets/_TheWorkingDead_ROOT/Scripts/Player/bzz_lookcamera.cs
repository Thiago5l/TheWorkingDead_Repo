using UnityEngine;

public class BzzBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Hacer que mire a cámara manteniendo "arriba"
        transform.LookAt(
            transform.position + Camera.main.transform.forward,
            Camera.main.transform.up
        );
    }
}
