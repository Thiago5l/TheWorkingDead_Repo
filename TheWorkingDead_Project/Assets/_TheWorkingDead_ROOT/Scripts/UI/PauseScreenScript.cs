using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject pauseButtons;
    public GameObject optionsButtons;
    public static bool gamePaused = false;

    public GameObject optionspanel;

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
        StartCoroutine(wait());
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        pauseButtons.SetActive(false);
        optionsButtons.SetActive(false);
        gamePaused = false;
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
        Time.timeScale = 1;
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
        paperScript.onFliped();
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.6f);
        Debug.Log("carita feliz");
    }
    IEnumerator waitForAnim()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        optionsButtons.SetActive(true);
    }
}
