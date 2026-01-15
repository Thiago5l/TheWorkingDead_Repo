using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

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
        Debug.Log("dialogo iniciado");
        UniversalUI.SetActive(true);
        if (index < 0 || index >= conversations.Count) return;

        var data = conversations[index];

        currentConversation = data.conversation;
        currentConversationUI = data.UIconversation;

        if (currentConversationUI != null)
            currentConversationUI.SetActive(true);

        ConversationManager.Instance.StartConversation(currentConversation);
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
    Final();
    IniciarDialogo(1);
    }

}
