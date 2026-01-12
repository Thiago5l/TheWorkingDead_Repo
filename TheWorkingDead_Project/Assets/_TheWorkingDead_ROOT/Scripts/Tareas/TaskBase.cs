using UnityEngine;

public abstract class TaskBase : MonoBehaviour
{
    [Header("Interacción")]
    [SerializeField] protected Material mat;
    [SerializeField] protected Material outline;
    [SerializeField] protected GameObject canvasInteractKey;
    [SerializeField] protected GameObject player;

    protected bool playerCerca;
    protected bool tareaAcabada;

    protected virtual void Start()
    {
        canvasInteractKey.SetActive(false);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && !tareaAcabada)
        {
            playerCerca = true;
            GetComponent<MeshRenderer>().material = outline;
            canvasInteractKey.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playerCerca = false;
            GetComponent<MeshRenderer>().material = mat;
            canvasInteractKey.SetActive(false);
        }
    }

    public void Interactuar()
    {
        if (playerCerca && !tareaAcabada)
            IniciarTarea();
    }

    protected abstract void IniciarTarea();
    protected abstract void CancelarTarea();

    protected void Completar()
    {
        tareaAcabada = true;
        player.GetComponent<PlayerController>().playerOcupado = false;
        canvasInteractKey.SetActive(false);
        GetComponent<MeshRenderer>().material = mat;
        this.enabled = false;
    }
}
