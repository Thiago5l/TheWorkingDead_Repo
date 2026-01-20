#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[InitializeOnLoad] // Para que funcione en modo editor
public class ColliderClickSelector : MonoBehaviour
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Pickable)]
    static void DrawColliderGizmos(Collider col, GizmoType gizmoType)
    {
        Gizmos.color = Color.green;

        if (col is BoxCollider box)
            Gizmos.DrawWireCube(box.transform.position + box.center, box.size);
        else if (col is SphereCollider sphere)
            Gizmos.DrawWireSphere(sphere.transform.position + sphere.center, sphere.radius);

        // Hace clic en el gizmo para seleccionar
        if (Handles.Button(col.transform.position + col.bounds.center, Quaternion.identity, 0.1f, 0.1f, Handles.SphereHandleCap))
        {
            Selection.activeGameObject = col.gameObject;
        }
    }
}
#endif
