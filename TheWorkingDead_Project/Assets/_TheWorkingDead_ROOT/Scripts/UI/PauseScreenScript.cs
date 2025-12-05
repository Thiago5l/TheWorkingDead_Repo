using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseUI;
    public static bool gamePaused = false;
    public GameObject player;
    [SerializeField] private bool puedePausar;

    public GameObject mainpanel;
    public GameObject optionspanel;
    [SerializeField] public PlayerController playerController;


    private void Update()
    {

        if (player.GetComponent<PlayerController>().playerOcupado == false) 
        {
            puedePausar = true;
        }

        if (player.GetComponent<PlayerController>().playerOcupado == true)
        {
            puedePausar = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && puedePausar)
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
        if (puedePausar == false) { return; }

    }

    public void IsPause()
    {

        if (puedePausar)
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            gamePaused = true;
        }
        
    }
    public void unPause()
    {
        if (puedePausar)
        {
            StartCoroutine(wait());
            Time.timeScale = 1;
            pauseUI.SetActive(false);
            gamePaused = false;
        }
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
    }
}
