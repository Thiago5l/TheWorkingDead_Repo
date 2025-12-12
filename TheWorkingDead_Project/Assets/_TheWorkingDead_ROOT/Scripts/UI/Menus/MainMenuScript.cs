using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManager;

    public GameObject mainpanel;
    public GameObject optionspanel;

    public void PlayButton()
    {
        StartCoroutine(wait("SCN_Test_Felix"));
    }

IEnumerator wait(string sceneName)
    {
        fadeManager.DoFade(0, 0.7f, 0.7f, 0);
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(sceneName);
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
    public void QuitGame()
    {
        Application.Quit();
    }
}
