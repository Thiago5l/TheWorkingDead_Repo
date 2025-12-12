using UnityEngine;


public class LightTrigger : MonoBehaviour
{
    public Light targetLight;          // Luz a activar
    public string playerTag = "Player"; // Tag del jugador

    void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        if (targetLight != null)
            targetLight.enabled = false; // Comienza apagada
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && targetLight != null)
        {
            targetLight.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag) && targetLight != null)
        {
            targetLight.enabled = false;
        }
    }
}

