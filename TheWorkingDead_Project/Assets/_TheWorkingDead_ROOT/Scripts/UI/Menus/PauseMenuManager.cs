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
    [SerializeField] Image actualSprite;
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

    public void OptionsButtons()
    {
        mainButtonGroup.SetActive(false);
        StartCoroutine(PaperOptionsManagement());
    }
    IEnumerator PaperOptionsManagement()
    {
        //This one is normal
        actualSprite.sprite = paperSprites[0];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[1];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[2];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[3];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[0];

        yield return new WaitForSecondsRealtime(0.1f);
        optionsButtonGroup.SetActive(true);
    }
    //-------//
    public void BackButton()
    {
        optionsButtonGroup.SetActive(false);
        controlsButtonGroup.SetActive(false);
        StartCoroutine(BackAnimationManager());
    }
    IEnumerator BackAnimationManager()
    {
        //This is Fliped
        actualSprite.sprite = paperSprites[0];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[3];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[2];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[1];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[0];

        yield return new WaitForSecondsRealtime(0.1f);
        mainButtonGroup.SetActive(true);
    }
    //-------//
    public void ControlsButtons()
    {
        mainButtonGroup.SetActive(false);
        StartCoroutine(ControlsAnimationManager());
    }
    IEnumerator ControlsAnimationManager()
    {
        actualSprite.sprite = paperSprites[0];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[1];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[2];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[3];
        yield return new WaitForSecondsRealtime(0.2f);
        actualSprite.sprite = paperSprites[0];

        yield return new WaitForSecondsRealtime(0.1f);
        controlsButtonGroup.SetActive(true);
    }
    //-------//
}
