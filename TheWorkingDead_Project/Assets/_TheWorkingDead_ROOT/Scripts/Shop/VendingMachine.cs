using UnityEngine;
using UnityEngine.InputSystem;

public class VendingMachine : MonoBehaviour
{
    public bool shopActive;

    [SerializeField] private GameObject shopCanvas;
    [SerializeField] PlayerController PlayerController;

    private void Start()
    {
        shopCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            shopActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            shopActive = false;
            shopCanvas.SetActive(false); // Cierra al salir
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
}
