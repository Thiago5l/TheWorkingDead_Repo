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

    public GameObject barra;
    public GameObject enemy;

    public Image buzzsprite;

    private int index;
    private bool startDialoguePlayed = false;
    private bool interactDialoguePlayed = false;

    private Outline outline;
    public float outlineBlinkSpeed = 3f;

    private Coroutine typeCoroutine;
    private Coroutine blinkCoroutine;
    private float originalFixedDeltaTime;

    void OnEnable()
    {
        buzzsprite.enabled = true;

        outline = buzzsprite.GetComponent<Outline>();
        if (outline == null)
            outline = buzzsprite.gameObject.AddComponent<Outline>();

        blinkCoroutine = StartCoroutine(BlinkOutlineCoroutine());

        textComponent.text = "";
    }

    IEnumerator BlinkOutlineCoroutine()
    {
        float alpha = outline.effectColor.a;
        int dir = -1;

        while (gameObject.activeInHierarchy)
        {
            alpha += dir * outlineBlinkSpeed * Time.unscaledDeltaTime;

            if (alpha <= 0f) { alpha = 0f; dir = 1; }
            else if (alpha >= 1f) { alpha = 1f; dir = -1; }

            Color c = outline.effectColor;
            c.a = alpha;
            outline.effectColor = c;

            yield return null;
        }
    }

    void Update()
    {
        if (currentLines == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == currentLines[index])
            {
                NextLine();
            }
            else
            {
                if (typeCoroutine != null) StopCoroutine(typeCoroutine);
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

        gameObject.SetActive(true);
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

            if (enemy != null) enemy.SetActive(true);
            if (barra != null) barra.GetComponent<OviedadZombie>().enabled = true;

            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);

            gameObject.SetActive(false);
            currentLines = null;
        }
    }
}
