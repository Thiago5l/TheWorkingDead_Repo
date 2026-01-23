using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinCanvasManager : MonoBehaviour
{
    public PlayerController PlayerController;
    public float duration = 0.5f; // Duración de la animación

    private RectTransform panel;
    private Vector2 startPos;
    private Vector2 endPos;
    public float offset = 900f;
    private void Awake()
    {
        // Toma automáticamente el RectTransform del mismo objeto
        panel = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;

        panel = GetComponent<RectTransform>();

        StartCoroutine(EntradaDesdeAbajo());
    }
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            PlayerController.coins++;
            PlayerController.FromPLayerToPLayerData();
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("No hay más escenas en el Build");
        }
    }
    IEnumerator EntradaDesdeAbajo()
    {
        //  esperar a que el Canvas calcule layout
        yield return null;
        yield return new WaitForEndOfFrame();

        // ahora sí existe la posición real
        endPos = panel.anchoredPosition;

        startPos = endPos + Vector2.down * offset;
        panel.anchoredPosition = startPos;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        panel.anchoredPosition = endPos;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
