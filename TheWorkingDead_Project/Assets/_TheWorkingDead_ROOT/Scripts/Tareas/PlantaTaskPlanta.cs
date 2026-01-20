using System.Collections;
using UnityEngine;

public class MoverEntreEmptys : MonoBehaviour
{
    [Header("Mover")]
    public Transform[] puntos;   // Los emptys
    public float velocidad = 3f; // Velocidad de movimiento
    public float tiempoEspera = 1f; // Tiempo entre cambios

    private int indiceActual = 1;
    private bool moviendo = false;
    [Header("Regar")]
    public float tiempoRegada = 5f;
    private bool estaRegada = false;
    private float temporizador;
    void Start()
    {
        // Coloca el objeto directamente en el punto 2 al empezar
        if (puntos.Length > indiceActual)
        {
            transform.position = puntos[indiceActual].position;
        }
    }
    void Update()
    {
        if (puntos.Length == 0)
        {
            if (!moviendo)
            {
                StartCoroutine(Mover());
            }
        }
        if (estaRegada)
        {
            temporizador -= Time.deltaTime;

            if (temporizador <= 0)
            {
                estaRegada = false;
                Debug.Log("La planta necesita agua");
            }
        }
    }
    #region regar
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agua"))
        {
            Regar();
        }
    }

    void Regar()
    {
        estaRegada = true;
        temporizador = tiempoRegada;

        Debug.Log("Planta regada");
    }
    #endregion
    #region mover
    IEnumerator Mover()
    {
        moviendo = true;

        Transform destino = puntos[indiceActual];

        while (Vector3.Distance(transform.position, destino.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destino.position,
                velocidad * Time.deltaTime
            );

            yield return null;
        }

        yield return new WaitForSeconds(tiempoEspera);

        indiceActual++;

        if (indiceActual >= puntos.Length)
            indiceActual = 0;

        moviendo = false;
    }
    #endregion
}

