using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCsConversation : MonoBehaviour
{
    [SerializeField] public NPCConversation myConversation;
    [SerializeField] private bool playerCerca;
    [SerializeField] private bool canInteract = true;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject taskExclamation;
    //[SerializeField] public GameObject objectTareas; 
    //private TareasAleatorias tareasScript;

    void Start()
    {
        taskExclamation.SetActive(true);
        playerCerca = false;
        canInteract = true;
    }

    void Update()
    {
        if (playerCerca && canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && canInteract)
        {
            playerCerca = true;
            Debug.Log("Jugador cerca del NPC");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playerCerca = false;
            Debug.Log("Jugador se alejó del NPC");
        }
    }

    public void Interact()
    {
        if (playerCerca && myConversation != null)
        {

            Debug.Log("aaaaaaaaaaAAAAAAAAAAAAAAAAaaaaaaaaaaa");
            ConversationManager.Instance.StartConversation(myConversation);
        }
        else
        {
            Debug.LogWarning("No hay conversación asignada");
        }
    }

    public void FinalBueno()
    {
        player.GetComponent<OviedadZombie>().Zombiedad -= (20f / 100f);
        taskExclamation.SetActive(false);
    }

    public void FinalMalo()
    {
        player.GetComponent<OviedadZombie>().Zombiedad += (20f / 100f);
    }

}