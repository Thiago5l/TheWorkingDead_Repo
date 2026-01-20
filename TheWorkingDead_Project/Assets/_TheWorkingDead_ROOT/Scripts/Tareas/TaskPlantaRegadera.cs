using UnityEngine;
using UnityEngine.UI;

public class TaskPlantaRegadera : MonoBehaviour
{
    public GameObject gotaPrefab;
    public Transform puntoSalida;
    public float cantidadMaxima = 10f;
    public float gastoPorSegundo = 1f;

    public float intervaloGotas = 0.05f; // tiempo entre gotas
    private float tiempoProximaGota = 0f;

    private float aguaActual;

    void Start()
    {
        aguaActual = cantidadMaxima;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && aguaActual > 0)
        {
            Regar();
        }
    }

    void Regar()
    {
        // Gastar agua
        aguaActual -= gastoPorSegundo * Time.deltaTime;
        if (aguaActual <= 0) aguaActual = 0;

        // Instanciar gotas según intervalo
        if (Time.time >= tiempoProximaGota)
        {
            tiempoProximaGota = Time.time + intervaloGotas;

            GameObject gota = Instantiate(gotaPrefab, puntoSalida.position, Quaternion.identity);
            // Destruir la gota después de 3 segundos
            Destroy(gota, 3f);

            // Opcional: agregar velocidad inicial aleatoria para dispersión
            Rigidbody rb = gota.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 velocidad = Vector3.down; // cae hacia abajo
                // dispersión ligera
                velocidad += new Vector3(
                    Random.Range(-0.1f, 0.1f),
                    0,
                    Random.Range(-0.1f, 0.1f)
                );
                rb.linearVelocity = velocidad * 5f; // ajusta velocidad
            }
        }
    }
}