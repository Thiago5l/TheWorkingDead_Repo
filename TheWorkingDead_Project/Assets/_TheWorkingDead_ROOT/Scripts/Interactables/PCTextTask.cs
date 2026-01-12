using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PCTextTask : TaskBase
{
    [Header("Text")]
    [SerializeField] private List<string> textsToSelect = new List<string>();
    [SerializeField] private int sizeTextsSelected = 3;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private TMP_InputField insertedText;

    [Header("Time")]
    [SerializeField] private Slider timeSlider;
    [SerializeField] public float timePerCharacter = 0.5f;

    [Header("Feedback")]
    [SerializeField] private FadeCanvas taskFeedbackCanvas;

    private List<string> textsSelected = new List<string>();
    private string textToWrite = string.Empty;
    private string currentText = string.Empty;

    private float timeMax;
    private float timeCurrent;

    protected override void Start()
    {
        base.Start();
        GenerateTextList();
    }

    protected override void IniciarTarea()
    {
        uiTarea.SetActive(true);

        insertedText.text = string.Empty;
        insertedText.ActivateInputField();

        NextWord();
    }

    protected override void CancelarTarea()
    {
        CancelarBase();
        ResetTimer();
    }

    private void Update()
    {
        if (!interactuando || tareaAcabada) return;

        timeCurrent -= Time.deltaTime;
        timeSlider.value = timeCurrent;

        if (timeCurrent <= 0f)
        {
            FailTask();
        }
    }

    private void LateUpdate()
    {
        if (!interactuando || tareaAcabada) return;

        currentText = insertedText.text;
        UpdateColoredText();

        if (string.IsNullOrEmpty(currentText)) return;
        if (currentText.Length > textToWrite.Length)
        {
            FailTask();
            return;
        }

        for (int i = 0; i < currentText.Length; i++)
        {
            if (char.ToLower(currentText[i]) != char.ToLower(textToWrite[i]))
            {
                FailTask();
                return;
            }
        }

        if (currentText.Length == textToWrite.Length)
        {
            textsSelected.RemoveAt(0);

            if (textsSelected.Count > 0)
                NextWord();
            else
                CompleteTextTask();
        }
    }

    private void GenerateTextList()
    {
        textsSelected.Clear();

        if (textsToSelect.Count < sizeTextsSelected)
        {
            Debug.LogError("Not enough texts in textsToSelect");
            return;
        }

        ShuffleList(textsToSelect);

        for (int i = 0; i < sizeTextsSelected; i++)
            textsSelected.Add(textsToSelect[i]);
    }

    private void NextWord()
    {
        insertedText.text = string.Empty;
        currentText = string.Empty;

        textToWrite = textsSelected[0];
        textUI.text = textToWrite;
        SetupTimer();
    }

    private void SetupTimer()
    {
        timeMax = textToWrite.Length * timePerCharacter;
        timeCurrent = timeMax;

        timeSlider.maxValue = timeMax;
        timeSlider.value = timeMax;
    }

    private void ResetTimer()
    {
        timeCurrent = timeMax;
        timeSlider.value = timeMax;
    }

    private void FailTask()
    {
        taskFeedbackCanvas.PlayLose();
        CancelarTarea();
    }

    private void CompleteTextTask()
    {
        taskFeedbackCanvas.PlayWin();
        uiTarea.SetActive(false);
        CompletarTarea();
    }

    private void UpdateColoredText()
    {
        string result = string.Empty;

        for (int i = 0; i < textToWrite.Length; i++)
        {
            if (i < currentText.Length)
            {
                result += (char.ToLower(currentText[i]) == char.ToLower(textToWrite[i]))
                    ? "<color=green>" + textToWrite[i] + "</color>"
                    : "<color=red>" + textToWrite[i] + "</color>";
            }
            else
            {
                result += "<color=white>" + textToWrite[i] + "</color>";
            }
        }

        textUI.text = result;
    }

    private void ShuffleList(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
