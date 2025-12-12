using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCsConversation : MonoBehaviour
{
    [SerializeField] public List<NPCConversation> conversationsList = new List<NPCConversation>();
    [SerializeField] public NPCConversation myConversation;
    [SerializeField] private bool playerCerca;
    [SerializeField] private bool canInteract = true;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject taskExclamation;
    [SerializeField] private bool talking;
    [SerializeField] private bool alrreadyTalked;
    //[SerializeField] public GameObject objectTareas; 
    //private TareasAleatorias tareasScript;

    void Start()
    {
        
        taskExclamation.SetActive(true);
        playerCerca = false;
        canInteract = true;
        alrreadyTalked = false;
        talking = false;
        MezclarLista(conversationsList);
        //myConversation = conversationsList[0];


    }

    void Update()
    {
        myConversation = conversationsList[0];
        //if (playerCerca && canInteract && Input.GetKeyDown(KeyCode.E))
        //{
        //    Interact();
        //}
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TaskPlayer") && canInteract)
        {
            playerCerca = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TaskPlayer"))
        {
            playerCerca = false;
        }
    }

    public void Interact()
    {
        if (playerCerca && myConversation != null && !talking)
        {

            MezclarLista(conversationsList);
            
            ConversationManager.Instance.StartConversation(myConversation);
            talking = true;
            player.GetComponent<PlayerController>().playerOcupado = true;

        }
        else
        {
            Debug.LogWarning("No hay conversación asignada");
        }
    }

    public void FinalBueno()
    {
        if (!alrreadyTalked)
        {
            player.GetComponent<OviedadZombie>().Zombiedad -= (20f / 100f);
            taskExclamation.SetActive(false);
            player.GetComponent<PlayerController>().playerOcupado = false;
            talking = false;
            alrreadyTalked = true;

        }
        else
        {
            player.GetComponent<PlayerController>().playerOcupado = false;
            talking = false;
            alrreadyTalked = true;
            
        }
    }

    public void FinalMalo()
    {
        if (!alrreadyTalked)
        {
            player.GetComponent<OviedadZombie>().Zombiedad += (20f / 100f);
            player.GetComponent<PlayerController>().playerOcupado = false;
            talking = false;
            alrreadyTalked = true;
            
        }
        else
        {
            player.GetComponent<PlayerController>().playerOcupado = false;
            talking = false;
            alrreadyTalked = true;
            
        }
    }


    


    private void MezclarLista(List<NPCConversation> list)
    {
        if (list.Count == 1) { return; }    
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            NPCConversation temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

}