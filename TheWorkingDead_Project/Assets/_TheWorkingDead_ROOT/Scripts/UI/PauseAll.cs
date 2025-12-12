using System.Collections;
using UnityEngine;

public class PauseAll : MonoBehaviour
{
    private void Start()
    {
        // Inicia la corutina
        StartCoroutine(ExecuteAfterDelay(2f));
    }

    private IEnumerator ExecuteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Pausar el juego
        StopAllTime();
    }

    void StopAllTime()
    {
        Time.timeScale = 0f; // Pausa el juego
    }
}
