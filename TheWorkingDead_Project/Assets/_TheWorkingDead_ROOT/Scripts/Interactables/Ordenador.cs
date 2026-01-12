using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OrdenadorTask : TaskBase
{
    [Header("Bar")]
    [SerializeField] private float startValue = 25f;
    [SerializeField] private float sumValue = 10f;
    [SerializeField] private float restValue = 5f;
    [SerializeField] private float tickTime = 0.5f;
    [SerializeField] private Image taskBar;

    [Header("Win Zone")]
    [SerializeField] private float winThreshold = 60f;
    [SerializeField] private float winZoneMin = 65f;
    [SerializeField] private float winZoneMax = 80f;

    [Header("Feedback")]
    [SerializeField] private Image spacebarSprite;
    [SerializeField] private float flashTime = 0.2f;
    [SerializeField] private FadeCanvas taskFeedbackCanvas;

    private float value;
    private float winValue;

    protected override void Start()
    {
        base.Start();
        value = startValue;
        taskBar.fillAmount = startValue;
        taskBar.gameObject.SetActive(false);
    }

    protected override void IniciarTarea()
    {
        value = startValue;
        winValue = 0f;

        taskBar.gameObject.SetActive(true);
        UpdateBar();

        StartCoroutine(DrainRoutine());
    }

    protected override void CancelarTarea()
    {
        CancelarBase();

        value = startValue;
        winValue = 0f;

        taskBar.gameObject.SetActive(false);
        UpdateBar();
    }

    private void Update()
    {
        if (!interactuando || tareaAcabada) return;

        UpdateBar();
        CheckWinZone();
        CheckFail();
    }

    public void TaskCode()
    {
        if (!interactuando || tareaAcabada) return;

        value += sumValue;
        value = Mathf.Clamp(value, 0f, 100f);

        StartCoroutine(FlashRoutine());
        UpdateBar();
    }

    private IEnumerator DrainRoutine()
    {
        while (interactuando && value > 0f && value < 100f)
        {
            yield return new WaitForSeconds(tickTime);
            value -= restValue;
            UpdateBar();
        }
    }

    private void CheckWinZone()
    {
        if (value > winZoneMin && value < winZoneMax)
        {
            winValue += Time.deltaTime;

            if (winValue >= winThreshold)
            {
                CompleteOrdenadorTask();
            }
        }
        else
        {
            winValue = 0f;
        }
    }

    private void CheckFail()
    {
        if (value <= 0f)
        {
            taskFeedbackCanvas.PlayLose();
            CancelarTarea();
        }
    }

    private void CompleteOrdenadorTask()
    {
        taskFeedbackCanvas.PlayWin();
        taskBar.gameObject.SetActive(false);
        CompletarTarea();
    }

    private void UpdateBar()
    {
        taskBar.fillAmount = Mathf.Clamp01(value / 100f);
    }

    private IEnumerator FlashRoutine()
    {
        spacebarSprite.fillAmount = 1f;
        yield return new WaitForSeconds(flashTime);
        spacebarSprite.fillAmount = 0f;
    }
}
