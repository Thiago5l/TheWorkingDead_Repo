using UnityEngine;

public class TareaMear : MonoBehaviour
{

    [SerializeField] private bool playerCerca;
    public GameObject canvasInteractableKey;




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            Debug.Log("Entra");
            playerCerca = true;
            canvasInteractableKey.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            Debug.Log("Sale");
            playerCerca = false;
            canvasInteractableKey.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Interactuar()
    {
        if (playerCerca)
        {

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
