using UnityEngine;

public class EnemyVisionCone : MonoBehaviour
{
    public float rango = 5f;     // largo del cono
    public float angulo = 45f;   // apertura del cono

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        // origen del cono
        Vector3 origen = transform.position;
        Vector3 forward = transform.forward;

        // calcula los dos bordes del cono
        Quaternion rotIzq = Quaternion.AngleAxis(-angulo, Vector3.up);
        Quaternion rotDer = Quaternion.AngleAxis(angulo, Vector3.up);

        Vector3 bordeIzq = rotIzq * forward * rango;
        Vector3 bordeDer = rotDer * forward * rango;

        // dibuja las líneas principales del cono
        Gizmos.DrawLine(origen, origen + bordeIzq);
        Gizmos.DrawLine(origen, origen + bordeDer);

        // dibuja líneas internas (opcional, para mejor visual)
        int pasos = 10;
        for (int i = 0; i <= pasos; i++)
        {
            float t = (float)i / pasos;
            float a = Mathf.Lerp(-angulo, angulo, t);

            Quaternion rotPaso = Quaternion.AngleAxis(a, Vector3.up);
            Vector3 direccion = rotPaso * forward * rango;

            Gizmos.DrawLine(origen, origen + direccion);
        }
    }
}
