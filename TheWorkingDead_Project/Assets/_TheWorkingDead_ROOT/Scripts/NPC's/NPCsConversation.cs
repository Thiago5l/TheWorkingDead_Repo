using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class NPCConversationTask : TaskBase
{
    [Header("Conversations")]
    [SerializeField] private List<NPCConversation> conversationsList = new List<NPCConversation>();
    [SerializeField] private NPCConversation myConversation;

    [Header("Feedback")]
    [SerializeField] private FadeCanvas taskFeedbackCanvas;

    [Header("Extras")]
    [SerializeField] public float rotationSpeed = 5f;

    private bool talking = false;
    private bool alreadyTalked = false;
    private bool girando = false;

    protected void Awake()
    {

        if (player == null)
            player = GameObject.FindWithTag("Player");

        if (taskManager == null)
            taskManager = FindAnyObjectByType<TareasAleatorias>();
    }

    protected override void Start()
    {
        base.Start();

        if (taskFeedbackCanvas == null)
            taskFeedbackCanvas = FindAnyObjectByType<FadeCanvas>();

        if (taskExclamation != null) taskExclamation.SetActive(true);

        MezclarLista(conversationsList);
    }

    private void Update()
    {
        // Mostrar/hide exclamacion y canvas
        if (playerCerca && !alreadyTalked && !tareaAcabada)
        {
            if (taskExclamation != null) taskExclamation.SetActive(true);
            canvasInteractKey?.SetActive(true);
        }
        else
        {
            canvasInteractKey?.SetActive(false);
            if (taskExclamation != null) taskExclamation.SetActive(!alreadyTalked && !tareaAcabada);
        }

        // Girar hacia el jugador
        if (girando && player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f;

            if (direction.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    transform.rotation = targetRotation;
                    girando = false;
                }
            }
        }
    }

    protected override void IniciarTarea()
    {
        girando = true;
        canvasInteractKey?.SetActive(false);

        // Mezclar y seleccionar conversacion
        MezclarLista(conversationsList);
        myConversation = conversationsList[0];

        if (player == null)
        {
            Debug.LogError("Player no asignado en NPCConversationTask");
            return;
        }

        // Activar playerOcupado
        var controller = player.GetComponent<PlayerController>();
        if (controller != null)
            controller.playerOcupado = true;

        // Iniciar la conversacion
        if (ConversationManager.Instance != null && myConversation != null)
        {
            ConversationManager.Instance.StartConversation(myConversation);
        }

        talking = true;
    }

    protected override void CancelarTarea()
    {
        CancelarBase();
        talking = false;
        girando = false;

        var controller = player?.GetComponent<PlayerController>();
        if (controller != null)
            controller.playerOcupado = false;
    }

    public void FinalBueno()
    {
        if (tareaAcabada) return;

        girando = false;
        talking = false;
        alreadyTalked = true;
        tareaAcabada = true;

        Win();
    }

    public void FinalMalo()
    {
        if (tareaAcabada) return;

        girando = false;
        talking = false;

        Loose();

        var controller = player?.GetComponent<PlayerController>();
        if (controller != null)
            controller.playerOcupado = false;
    }

    private void MezclarLista(List<NPCConversation> list)
    {
        if (list.Count <= 1) return;

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            NPCConversation temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("TaskPlayer"))
            playerCerca = true;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag("TaskPlayer"))
            playerCerca = false;
    }
}
