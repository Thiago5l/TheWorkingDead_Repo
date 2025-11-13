using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;
    public static bool gamePaused = false;

    public GameObject mainpanel;
    public GameObject optionspanel;

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
        gamePaused = true;
    }
    public void unPause()
    {
        StartCoroutine(wait());
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        gamePaused = false;
    }
    public void BackMenu()
    {
        StartCoroutine(wait());
        Debug.Log("loading main scene");
        SceneManager.LoadScene("SCN_MainMenu_Test");
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
        StartCoroutine(SetActive());
    }
    public void OptionsBack()
    {
        StartCoroutine(SetBack());
    }
    IEnumerator SetActive()
    {
        yield return new WaitForSeconds(0.6f);
        mainpanel.SetActive(false);
        yield return 0;
        optionspanel.SetActive(true);
    }
    IEnumerator SetBack()
    {
        yield return new WaitForSeconds(0.6f);
        mainpanel.SetActive(true);
        yield return new WaitForSeconds(0);
        optionspanel.SetActive(false);
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.6f);
        Debug.Log("carita feliz");
    }
}
