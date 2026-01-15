using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
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
    private GameObject currentConversationUI;
    private NPCConversation currentConversation;

    [Header("Player")]
    [SerializeField] private GameObject player;

    private void Start()
    {
        IniciarDialogo(0);
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
        yield return null;
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
    public void PlayZombiedad()
    {
    IniciarDialogo(1);
    }
    public void PlayTareas()
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

        // Inicia la nueva conversación (índice 2)
        IniciarDialogo(2);
    }


}
