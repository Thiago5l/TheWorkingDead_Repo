#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ColliderClickSelector
{
    static ColliderClickSelector()
    {
        // Suscribirse al evento de Scene GUI
        SceneView.duringSceneGui += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        // Detectar clic izquierdo
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Selecciona el GameObject que tenga un collider
                Selection.activeGameObject = hit.collider.gameObject;
                e.Use(); // Evita que otros eventos lo procesen
            }
        }
    }
}
#endif
