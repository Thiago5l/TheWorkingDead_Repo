using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject pauseButtons;
    public GameObject optionsButtons;
    public GameObject controlsButtons;
    public static bool gamePaused = false;

    public PaperAnimManager paperScript;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                unPause();
            }
            else
            {
                IsPause();
            }
        }

    }

    public void IsPause()
    {
        Time.timeScale = 0;

        pauseUI.SetActive(true);
        pauseButtons.SetActive(true);

        gamePaused = true;

        paperScript.onPause();
    }
    public void unPause()
    {
        paperScript.noLongerPaused();
        StartCoroutine(noPauseTime());
    }
    public void BackMenu()
    {
        StartCoroutine(wait());
        Debug.Log("loading main scene");
        SceneManager.LoadScene("SCN_Test_MainMenu");
        Time.timeScale = 1;
    }
    public void RestartLv()
    {
        StartCoroutine(wait());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("restart");
    }
    public void Options()
    {
        pauseButtons.SetActive(false);
        paperScript.onFliped();
        StartCoroutine(waitForAnim());
    }
    public void OptionsBack()
    {
        pauseButtons.SetActive(true);
        optionsButtons.SetActive(false);
        paperScript.onReverseFliped();
    }
    public void controls()
    {
        optionsButtons.SetActive(false);
        StartCoroutine(waitForControls());
        paperScript.onFliped();
    }
    IEnumerator noPauseTime()
    {
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;

        pauseUI.SetActive(false);
        pauseButtons.SetActive(false);
        optionsButtons.SetActive(false);
        controlsButtons.SetActive(false);

        gamePaused = false;
    }
    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        Debug.Log("carita feliz");
    }
    IEnumerator waitForAnim()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        optionsButtons.SetActive(true);
    }
    IEnumerator waitForControls()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        controlsButtons.SetActive(true);
    }
}
