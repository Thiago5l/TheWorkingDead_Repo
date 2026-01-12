using UnityEngine;

public abstract class TaskBase : MonoBehaviour
{
    [Header("Interacción")]
    [SerializeField] protected Material mat;
    [SerializeField] protected Material outline;
    [SerializeField] protected GameObject canvasInteractKey;
    [SerializeField] protected GameObject player;
    [SerializeField] Renderer objRenderer;
    [SerializeField] Color colorCerca = Color.green;
    [SerializeField] Color colorLejos = Color.black;

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
            CambiarColorOutline(colorCerca);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playerCerca = false;
            GetComponent<MeshRenderer>().material = mat;
            canvasInteractKey.SetActive(false);
            CambiarColorOutline(colorLejos);
        }
    }
    void CambiarColorOutline(Color color)
    {
        Material[] mats = objRenderer.materials;

        if (mats.Length > 1)
        {
            mats[1].SetColor("_ContourColor", color);
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
