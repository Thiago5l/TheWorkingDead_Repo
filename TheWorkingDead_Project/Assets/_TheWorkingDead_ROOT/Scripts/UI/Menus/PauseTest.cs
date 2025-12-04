using System.Collections;
using UnityEngine;

public class PauseTest : MonoBehaviour
{
    [SerializeField] Transform paper;
    [SerializeField] GameObject panel;
    [SerializeField] bool gamePaused = false;

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
    public void unPause()
    {
        paper.LeanMoveLocalY(-Screen.height, 1f).setEaseOutExpo().setIgnoreTimeScale(true).setOnComplete(OnceCompleted);
    }
    public void IsPause()
    {
        Time.timeScale = 0;

        panel.SetActive(true);
        gamePaused = true;
        paper.localPosition = new Vector2(0, -Screen.height);
        paper.LeanMoveLocalY(-60, 1f).setEaseOutExpo().setIgnoreTimeScale(true);
    }
    public void OnceCompleted()
    {
        panel.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1;
    }
}
