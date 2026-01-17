using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuzzTutorial : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public string[] startDialogue;
    public string[] zombiedadDialogue;
    public string[] energéticasDialogue;
    public string[] snacksDialogue;
    public string[] interactDialogue;
    public string[] tasksDialogue;
    public string[] npcDialogue;
    public string[] vendingDialogue;

    private string[] currentLines;

    public float textSpeed = 0.05f;

    public Image buzzsprite;

    private int index;
    private bool startDialoguePlayed = false;
    private bool interactDialoguePlayed = false;


    private Coroutine typeCoroutine;
    private Coroutine blinkCoroutine;
    private float originalFixedDeltaTime;

    public ActiveBuzz ActiveBuzz;

    [Header("Dialogue UI")]
    public GameObject startDialogueUI;
    public GameObject interactDialogueUI;
    public GameObject tasksDialogueUI;
    public GameObject npcDialogueUI;
    public GameObject vendingDialogueUI;

    private GameObject currentDialogueUI;

    void OnEnable()
    {

        textComponent.text = "";
    }

    void Update()
    {
        if (currentLines == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("CLICK DETECTADO");

            // Si ya terminó de escribir la línea
            if (textComponent.text.Trim() == currentLines[index].Trim())
            {
                NextLine();
            }
            else
            {
                // Completar línea instantáneo
                if (typeCoroutine != null)
                    StopCoroutine(typeCoroutine);

                textComponent.text = currentLines[index];
            }
        }
    }

    public void PlayStartDialogue()
    {
        if (!startDialoguePlayed)
        {
            startDialoguePlayed = true;
            currentDialogueUI = startDialogueUI;
            PlayDialogue(startDialogue);
        }
    }

    public void PlayInteractDialogue()
    {
        if (!interactDialoguePlayed)
        {
            interactDialoguePlayed = true;
            currentDialogueUI = interactDialogueUI;
            PlayDialogue(interactDialogue);
        }
    }

    void PlayDialogue(string[] linesToPlay)
    {
        currentLines = linesToPlay;
        index = 0;

        if (currentDialogueUI != null)
            currentDialogueUI.SetActive(true);

        textComponent.text = "";

        originalFixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f;

        typeCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textComponent.text = "";
        foreach (char c in currentLines[index])
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < currentLines.Length - 1)
        {
            index++;
            typeCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = originalFixedDeltaTime;
            currentLines = null;

            StartCoroutine(EndDialogueCoroutine());
        }
    }
    private IEnumerator EndDialogueCoroutine()
    {
        yield return new WaitForSecondsRealtime(1f); // espera 1 segundo

        if (currentDialogueUI != null)
            currentDialogueUI.SetActive(false);

        ActiveBuzz.objectActivated = false;
        this.gameObject.SetActive(false);
    }
}
