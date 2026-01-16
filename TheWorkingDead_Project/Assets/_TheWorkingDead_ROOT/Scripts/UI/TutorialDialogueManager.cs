using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class TutorialConversationUI
{
    public NPCConversation conversation;
    public GameObject UIconversation;
}
public class TutorialDialogueManager : MonoBehaviour
{
    [Header("Conversations")]
    [SerializeField] private List<TutorialConversationUI> conversations;

    [Header("UI")]
    [SerializeField] GameObject UniversalUI;
    [SerializeField] GameObject TasksUitutorial;
    private GameObject currentConversationUI;
    private NPCConversation currentConversation;

    [Header("References")]
    [SerializeField] PlayerController PlayerController;
    [SerializeField] VendingMachine VendingMachine;
    [SerializeField] CoinManager CoinManager;
    [SerializeField] VendingMachine vendingMachine;
    [Header("Flags")]
    public bool taskopened;
    public bool playedtareas;
    public bool playedvendingmachine;
    private void Start()
    {
        TasksUitutorial.SetActive(false);
        taskopened = false;
        PlayerController.playerOcupado=true;
        IniciarDialogo(0);
    }
    private bool dialogo3Iniciado = false;
    private bool dialogo4Iniciado = false;
    private bool dialogo5Iniciado = false;
    private void Update()
    {
        if (playedtareas && taskopened && !dialogo3Iniciado)
        {
            dialogo3Iniciado = true;
            Debug.Log("playeddialogos");
            PlayDialogos();
        }
        if (playedtareas && taskopened && PlayerController.tutorialroom && !dialogo4Iniciado)
        {
            dialogo4Iniciado=true;
            Debug.Log("playedtienda");
            PlayTienda();
        }
        if (Input.GetKeyDown(KeyCode.Tab) && !taskopened)
        {
            taskopened = true;
            if (currentConversationUI != null)
                currentConversationUI.SetActive(false);
        }
        if ((PlayerController.snacks > 0 || PlayerController.energeticas > 0) && !dialogo5Iniciado && !vendingMachine.shopCanvas.activeSelf)
        {
            dialogo5Iniciado = true;
            Debug.Log("playeditems");
            PlayItems();
        }

    }
    public void IniciarDialogo(int index = 0)
    {
        UniversalUI.SetActive(true);
        if (index < 0 || index >= conversations.Count) return;

        var data = conversations[index];
        currentConversation = data.conversation;
        currentConversationUI = data.UIconversation;

        if (currentConversationUI != null)
            currentConversationUI.SetActive(true);

        StartCoroutine(StartConversationNextFrame(currentConversation));
    }
    IEnumerator StartConversationNextFrame(NPCConversation conv)
    {
        while (ConversationManager.Instance.IsConversationActive)
        {
            yield return null;
        }

        ConversationManager.Instance.StartConversation(conv);
    }

    public void Final()
    {
        if (currentConversationUI != null)
            currentConversationUI.SetActive(false);

        currentConversationUI = null;
        currentConversation = null;
        UniversalUI.SetActive(false);
    }
    #region playdialogues
    public void PlayZombiedad()
    {
    IniciarDialogo(1);
    }
    public void PlayTareas()
    {
        StartCoroutine(CambiarDialogoCoroutine(2));
    }



    public void PlayDialogos()
    {
        // Finaliza la conversación actual si hay alguna activa
        if (currentConversationUI != null)
        {
            currentConversationUI.SetActive(false);
        }

        if (currentConversation != null)
        {
            ConversationManager.Instance.EndConversation(); // Finaliza la conversación actual
            currentConversation = null;
        }

        currentConversationUI = null;

        IniciarDialogo(3);
    }
    public void PlayTienda()
    {
        // Finaliza la conversación actual si hay alguna activa
        if (currentConversationUI != null)
        {
            currentConversationUI.SetActive(false);
        }

        if (currentConversation != null)
        {
            ConversationManager.Instance.EndConversation(); // Finaliza la conversación actual
            currentConversation = null;
        }

        currentConversationUI = null;

        IniciarDialogo(4);
    }
    public void PlayItems()
    {
        StartCoroutine(PlayItemsCoroutine());
    }

    IEnumerator PlayItemsCoroutine()
    {
        // Cierra UI actual
        if (currentConversationUI != null)
            currentConversationUI.SetActive(false);

        if (currentConversation != null)
        {
            ConversationManager.Instance.EndConversation();
            currentConversation = null;
        }

        currentConversationUI = null;

        // Espera un frame
        yield return null;

        // Inicia el diálogo
        IniciarDialogo(5);
    }
    IEnumerator CambiarDialogoCoroutine(int index)
    {
        if (ConversationManager.Instance.IsConversationActive)
            ConversationManager.Instance.EndConversation();

        while (ConversationManager.Instance.IsConversationActive)
            yield return null;

        if (currentConversationUI != null)
            currentConversationUI.SetActive(false);

        currentConversationUI = null;
        currentConversation = null;

        IniciarDialogo(index);
    }

    public void DesactivarUIDialogo(int index)
    {
        if (index < 0 || index >= conversations.Count)
            return;

        var ui = conversations[index].UIconversation;

        if (ui != null && ui.activeSelf)
            ui.SetActive(false);
    }

    #endregion
    #region flagvoids
    public void playerocupadofalse()
    { PlayerController.playerOcupado=false; }
    public void playerocupadotrue()
    { PlayerController.playerOcupado=true; }
    public void playedtareastrue()
    {  playedtareas=true; }
    public void taskuitutorial()
    { TasksUitutorial.SetActive(true); }
    public void changevendingmachinecolor()
    { VendingMachine.CambiarColorOutline(Color.yellow);
        PlayerController.coins=5;
    }
    public void playedvendingmachines()
    {
        playedvendingmachine=true;
    }
    public void Desactivarui1()
    { DesactivarUIDialogo(1); }
    
    #endregion


}
