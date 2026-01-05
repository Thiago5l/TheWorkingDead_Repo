using UnityEngine;
using UnityEngine.InputSystem;

public class AireAcondicionado : MonoBehaviour
{
    [SerializeField] GameObject particulas;
    [SerializeField] PlayerController playerController;
    public bool playercerca;

    bool activo = false;
    private void Awake()
    {
        if (playerController == null)
            playerController = FindAnyObjectByType<PlayerController>();
    }
    private void Start()
    {
        activo = false;
        if (particulas != null)
            particulas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playercerca = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playercerca = false;
        }
    }
    public void Apagar()
    {
        if (!activo || !playercerca || playerController == null) return;

        playerController.airesapagados++;
        Desactivar();
        Debug.Log("Aire apagado. Total: " + playerController.airesapagados);
    }
    public void Activar()
    {
        Debug.Log("tarea aires empezada");
        if (activo) return;

        activo = true;

        if (particulas != null)
            particulas.SetActive(true);

    }

    public void Desactivar()
    {
        if (!activo) return;

        activo = false;

        if (particulas != null)
            particulas.SetActive(false);
    }
}

