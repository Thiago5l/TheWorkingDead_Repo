using UnityEngine;

[ExecuteAlways]
public class ColliderVisualizer : MonoBehaviour
{
    void OnDrawGizmos()
    {
        // Usando la nueva API sin ordenamiento
        Collider[] colliders = Object.FindObjectsByType<Collider>(FindObjectsSortMode.None);
        Gizmos.color = Color.green;

        foreach (var col in colliders)
        {
            if (col is BoxCollider box)
                Gizmos.DrawWireCube(box.transform.position + box.center, box.size);
            else if (col is SphereCollider sphere)
                Gizmos.DrawWireSphere(sphere.transform.position + sphere.center, sphere.radius);
            else if (col is CapsuleCollider capsule)
            {
                // Dibujo aproximado para CapsuleCollider
                Gizmos.DrawWireSphere(capsule.transform.position + capsule.center + Vector3.up * (capsule.height / 2 - capsule.radius), capsule.radius);
                Gizmos.DrawWireSphere(capsule.transform.position + capsule.center - Vector3.up * (capsule.height / 2 - capsule.radius), capsule.radius);
            }
            // Si quieres, aquí puedes agregar MeshCollider u otros
        }
    }
}
