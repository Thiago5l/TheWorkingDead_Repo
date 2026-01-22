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

    private void Awake()
    {
        // Toma automáticamente el RectTransform del mismo objeto
        panel = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // Pausar el juego mientras el panel esté activo
        Time.timeScale = 0f;

        // Lanza la animación
        StartCoroutine(SlideUpFromBottom());
    }

    private IEnumerator SlideUpFromBottom()
    {
        // Espera un frame para que Unity calcule correctamente la posición final
        yield return null;

        // Guardar posición final y calcular posición inicial fuera de pantalla
        endPos = panel.anchoredPosition;
        startPos = endPos + Vector2.down * 800f; // Ajusta 800 según la distancia que quieras que venga de abajo
        panel.anchoredPosition = startPos;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;

            // Suavizado para efecto más natural
            float smooth = Mathf.SmoothStep(0f, 1f, t);
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, smooth);
            yield return null;
        }

        panel.anchoredPosition = endPos; // Asegura posición final exacta
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

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
