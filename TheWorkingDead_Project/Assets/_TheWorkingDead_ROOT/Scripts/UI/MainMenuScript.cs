using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{

    public GameObject mainpanel;
    public GameObject optionspanel;

    public void PlayButton()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.6f);
        Debug.Log("carita feliz");
        SceneManager.LoadScene("SCN_Toni_Map");
        //los nombres dde las escenas van segun estan puestas en el index de las build settings
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
        Debug.Log("bolas de queso"); //quitar esto tmb jeje
    }
}
