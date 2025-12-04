using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Panels")]

    [SerializeField] GameObject pausePanel;
    [SerializeField] Transform paperImage;

    [Header("Button Groups")]

    [SerializeField] GameObject mainButtonGroup;
    [SerializeField] GameObject optionsButtonGroup;
    [SerializeField] GameObject controlsButtonGroup;

    [Header("Values")]

    [SerializeField] bool gamePaused = false;
    [SerializeField] string Pedos = "muchos";

    [Header("Sprites")]

    [SerializeField] Sprite[] paperSprites;


    //-------//
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused == true)
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
        Time.timeScale = 0;

        gamePaused = true;
        pausePanel.SetActive(true);

        paperImage.localPosition = new Vector2(0, -Screen.height);
        paperImage.LeanMoveLocalY(0, 1f).setEaseOutExpo().setIgnoreTimeScale(true).delay = 0.1f;
    }
    public void unPause()
    {
        paperImage.LeanMoveLocalY(-Screen.height, 0.6f).setEaseInExpo().setIgnoreTimeScale(true).setOnComplete(OnceCompleted);
    }
    public void OnceCompleted()
    {
        pausePanel.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1;
    }
    //-------//

}
