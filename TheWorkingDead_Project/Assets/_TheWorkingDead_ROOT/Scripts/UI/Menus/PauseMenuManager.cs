using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Panels")]

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject paperImage;

    [Header("Button Groups")]

    [SerializeField] GameObject mainButtonGroup;
    [SerializeField] GameObject optionsButtonGroup;
    [SerializeField] GameObject controlsButtonGroup;

    [Header("Values")]

    [SerializeField] bool gamePaused;
    [SerializeField] int pedo;

    //-------//
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
    //-------//
    public void IsPause()
    {
        gamePaused = true;
        Time.timeScale = 0;

        pausePanel.SetActive(true);
        mainButtonGroup.SetActive(true);
        StartCoroutine(AnimationPaper());
    }
    public void unPause()
    {
        gamePaused = false;
    }
    //-------//
    IEnumerator AnimationPaper()
    {
        yield return new WaitForSecondsRealtime(1f);
        paperImage.transform.LeanMoveLocal(new Vector2(0, -80), 5).setEaseInCubic();

    }
}
