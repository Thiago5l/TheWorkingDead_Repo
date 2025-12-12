using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuzzTutorial : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public string[] startDialogue;
    public string[] interactDialogue;

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

    public void PlayInteractDialogue()
    {
        if (!interactDialoguePlayed)
        {
            Debug.Log("Playing interact dialogue");
            interactDialoguePlayed = true;
            PlayDialogue(interactDialogue);
        }
    }

    public void PlayStartDialogue()
    {
        if (!startDialoguePlayed)
        {
            Debug.Log("Playing start dialogue");
            startDialoguePlayed = true;
            PlayDialogue(startDialogue);
        }
    }

    void PlayDialogue(string[] linesToPlay)
    {
        currentLines = linesToPlay;
        index = 0;

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
            //Chat, añade aqui que espere 1 segundo

            StartCoroutine(SetActivatedCoroutine());
        }
    }
    private IEnumerator SetActivatedCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        ActiveBuzz.objectActivated = false;
        this.gameObject.SetActive(false);
    }
}
