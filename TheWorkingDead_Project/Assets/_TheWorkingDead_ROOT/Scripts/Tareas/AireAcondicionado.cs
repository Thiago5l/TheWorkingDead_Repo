using UnityEngine;
using UnityEngine.InputSystem;

public class AireAcondicionado : MonoBehaviour
{
    [Header("references")]
    [SerializeField] GameObject particulas;
    [SerializeField] GameObject taskkey;
    [SerializeField] PlayerController playerController;
    public bool playercerca;

    [Header("outline")]
    [SerializeField] Renderer objRenderer;
    [SerializeField] Color colorCerca = Color.green;
    [SerializeField] Color colorLejos = Color.black;

    bool activo = false;
    private void Awake()
    {
        if (playerController == null)
            playerController = FindAnyObjectByType<PlayerController>();
        if (objRenderer == null)
            objRenderer = GetComponent<Renderer>();
    }
    private void Start()
    {
        taskkey.SetActive(false);
        activo = false;
        if (particulas != null)
            particulas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playercerca = true;
            if (activo)
            {
                taskkey.SetActive(true);
            CambiarColorOutline(colorCerca);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playercerca = false;
            if (activo)
            {
                taskkey.SetActive(false);
                CambiarColorOutline(colorLejos);
            }
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
    void CambiarColorOutline(Color color)
    {
        Material[] mats = objRenderer.materials;

        if (mats.Length > 1)
        {
            mats[1].SetColor("_ContourColor", color);
        }
    }

    public void Desactivar()
    {
        if (!activo) return;

        activo = false;

        if (particulas != null)
            particulas.SetActive(false);
        taskkey.SetActive(false);
        CambiarColorOutline(colorLejos);
    }
}

