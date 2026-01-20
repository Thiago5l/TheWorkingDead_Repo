using UnityEngine;
using UnityEngine.InputSystem;

public class VendingMachine : MonoBehaviour
{
    public bool shopActive;

    [SerializeField] public GameObject shopCanvas;
    [SerializeField] PlayerController PlayerController;
    [Header("outline")]
    [SerializeField] Renderer objRenderer;
    [SerializeField] Color colorCerca = Color.yellow;
    [SerializeField] Color colorLejos = Color.black;
    [SerializeField] GameObject taskkey;
    private void Start()
    {
        if (objRenderer == null)
            objRenderer = GetComponentInChildren<Renderer>();
        shopCanvas.SetActive(false);
        taskkey.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            shopActive = true;
            taskkey.SetActive(true);
            CambiarColorOutline(colorCerca);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            shopActive = false;
            shopCanvas.SetActive(false); // Cierra al salir
            taskkey.SetActive(false);
            CambiarColorOutline(colorLejos);
        }
    }

    // Llamado por botón o tecla
    public void ActiveShop(InputAction.CallbackContext context)
    {
        if (shopActive) 
        { 
            shopCanvas.SetActive(true); 
            PlayerController.playerOcupado=true;
        }
    }
    public void DisableShop()
    {
        if (shopActive)
        {
            shopCanvas.SetActive(false);
            PlayerController.playerOcupado=false;
        }
    }
    public void CambiarColorOutline(Color color)
    {
        Material[] mats = objRenderer.materials;

        if (mats.Length > 1)
        {
            mats[1].SetColor("_ContourColor", color);
        }
    }
}
