using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCanvasManager : MonoBehaviour
{
    public GameObject winCanvas;
    public PlayerController PlayerController;
    private void OnEnable()
    {
        winCanvas.LeanMoveLocalY(0, 1f).setEaseOutExpo().setIgnoreTimeScale(true).delay = 0.1f;
        Time.timeScale = 0f;
    }
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        // Si existe la siguiente escena
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
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
