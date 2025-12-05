using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuzzText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    public GameObject barra;
    public GameObject enemy;

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        // PAUSAR EL JUEGO
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            // Usamos WaitForSecondsRealtime porque timeScale = 0
            yield return new WaitForSecondsRealtime(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            // REANUDAR EL JUEGO
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            // Reactivar objetos
            if (enemy != null) enemy.SetActive(true);
            if (barra != null) barra.GetComponent<OviedadZombie>().enabled = true;

            gameObject.SetActive(false);
        }
    }
}
