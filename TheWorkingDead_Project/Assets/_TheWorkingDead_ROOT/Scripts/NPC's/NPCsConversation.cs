using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.InputSystem;

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
    [SerializeField] public GameObject canvasinteractkey;
    [SerializeField] TareasAleatorias taskmanager;
    [SerializeField] private FadeCanvas taskFeedbackCanvas;
    public float rotationSpeed = 5f; // Velocidad de giro
    public bool girando = false;
    //[SerializeField] public GameObject objectTareas; 
    //private TareasAleatorias tareasScript;

    void Start()
    {
        myConversation = conversationsList[0];
        canvasinteractkey.SetActive(false);
        
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
        if (playerCerca&&!alrreadyTalked)
        {
            taskExclamation.SetActive(false);
            canvasinteractkey.SetActive(true);
        }
        else
        {
            if (!alrreadyTalked)
            {
                canvasinteractkey.SetActive(false);
                taskExclamation.SetActive(true); 
            }
            else
            {
                if (alrreadyTalked)
                {
                    taskExclamation.SetActive(false);
                    canvasinteractkey.SetActive(false);
                }
            }
        }
        if (girando && player != null)
        {
            // Calcula la dirección hacia el jugador solo en el plano XZ
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f; // Ignoramos la diferencia vertical

            if (direction.magnitude > 0.01f) // Evita errores al estar muy cerca
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Si está muy cerca de la rotación deseada, paramos de girar
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    transform.rotation = targetRotation;
                    girando = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E)) { Interact(); }
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
    public void InteractInput(InputAction.CallbackContext context)
    {
        Debug.Log("E PRESIONADA EN DIALOGO");
        Interact();
    }
    public void Interact()
    {
        Vector3 direction = player.transform.position - transform.position;
        // Crea la rotación deseada (mirando al jugador)
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // Aplica el giro suave
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        myConversation = conversationsList[0];
        if (playerCerca && myConversation != null && !talking && !alrreadyTalked)
        {
            girando = true;
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
        girando=false;
        taskFeedbackCanvas.PlayWin();
        Debug.Log("Final Bueno");

        player.GetComponent<PlayerController>().playerOcupado = false; 
            taskExclamation.SetActive(false);
            player.GetComponent<PlayerController>().playerOcupado = false;
            talking = false;
            alrreadyTalked = true;
            taskmanager.CompletarTarea(this.gameObject);

    }

    public void FinalMalo()
    {
        girando = false;
        taskFeedbackCanvas.PlayLose();
        Debug.Log("Final Malo");    
        player.GetComponent<PlayerController>().playerOcupado = false;
            talking = false;

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