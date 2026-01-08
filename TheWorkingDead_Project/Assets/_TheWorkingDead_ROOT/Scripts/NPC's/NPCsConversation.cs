using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCsConversation : MonoBehaviour
{
    [SerializeField] public List<NPCConversation> conversationsList = new List<NPCConversation>();
    [SerializeField] public NPCConversation myConversation;
    [SerializeField] private bool playerCerca;
    [SerializeField] private bool tieneTarea;
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

    private void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (taskFeedbackCanvas == null)
            taskFeedbackCanvas = FindAnyObjectByType<FadeCanvas>();
        if (taskmanager == null)
            taskmanager = FindAnyObjectByType<TareasAleatorias>();
    }
    void Start()
    {
        canvasinteractkey.SetActive(false);
        
        taskExclamation.SetActive(true);
        playerCerca = false;
        canInteract = true;
        alrreadyTalked = false;
        talking = false;
        MezclarLista(conversationsList);
        //myConversation = conversationsList[0];
        bool tieneTarea = EstaEnListaDeTareas();

    }

    void Update()
    {
        tieneTarea = EstaEnListaDeTareas();
        if (playerCerca&&!alrreadyTalked&&tieneTarea)
        {
            taskExclamation.SetActive(false);
            canvasinteractkey.SetActive(true);
        }
        else
        {
            if (!alrreadyTalked&&tieneTarea)
            {
                canvasinteractkey.SetActive(false);
                taskExclamation.SetActive(true); 
            }
            else
            {
                    taskExclamation.SetActive(false);
                    canvasinteractkey.SetActive(false);
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
    }
    public bool EstaEnListaDeTareas()
    {
        if (taskmanager == null)
        {
            Debug.LogWarning("TaskManager no asignado.");
            return false;
        }

        // Devuelve true si este GameObject está en la lista de tareas pendientes
        return taskmanager.OrdenTareas.Contains(this.gameObject);
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
        Debug.Log("hablando");

        if (player == null)
        {
            Debug.LogError("Player no asignado");
            return;
        }

        if (conversationsList == null || conversationsList.Count == 0)
        {
            Debug.LogError("No hay conversaciones");
            return;
        }

        myConversation = conversationsList[0];

        if (playerCerca && myConversation != null && !talking && !alrreadyTalked && tieneTarea)
        {
            girando = true;
            MezclarLista(conversationsList);

            if (ConversationManager.Instance != null)
                ConversationManager.Instance.StartConversation(myConversation);
            else
                Debug.LogError("ConversationManager.Instance es NULL");

            talking = true;

            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
                controller.playerOcupado = true;
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