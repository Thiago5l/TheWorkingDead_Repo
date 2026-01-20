#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class ColliderVisualizer : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Collider[] colliders = Object.FindObjectsByType<Collider>(FindObjectsSortMode.None);
        Gizmos.color = Color.green;

        foreach (var col in colliders)
        {
            if (col is BoxCollider box)
            {
                Gizmos.DrawWireCube(box.transform.position + box.center, box.size);

                // Esto permite hacer clic en la escena para seleccionar
                if (Handles.Button(box.transform.position + box.center, Quaternion.identity, 0.3f, 0.3f, Handles.CubeHandleCap))
                    Selection.activeGameObject = col.gameObject;
            }
        }
    }
}
#endif
